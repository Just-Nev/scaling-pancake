using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickToShoot : MonoBehaviour
{
    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    [Header("Activation")]
    public float activationRadius = 2f;

    [Header("Cooldown UI (Optional)")]
    public Image cooldownImage;

    [Header("Squash & Stretch")]
    public bool enableSquashStretch = true;
    public float squashAmount = 0.85f;     // smaller = more squash
    public float stretchAmount = 1.15f;    // bigger = more stretch
    public float squashTotalTime = 0.12f;  // total time for the whole punch

    [Header("Camera Shake")]
    public bool enableCameraShake = true;
    public float shakeDuration = 0.08f;
    public float shakeMagnitude = 0.15f;

    private float fireTimer;
    private bool isHolding;
    private Vector3 originalScale;
    private Camera cam;

    private Coroutine squashRoutine;
    private Coroutine shakeRoutine;

    void Start()
    {
        cam = Camera.main;
        originalScale = transform.localScale;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryActivate();

        if (Input.GetMouseButtonUp(0))
            isHolding = false;

        if (!isHolding) return;

        fireTimer += Time.deltaTime;

        if (cooldownImage != null)
            cooldownImage.fillAmount = fireTimer / fireRate;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;

            if (cooldownImage != null)
                cooldownImage.fillAmount = 0f;
        }
    }

    void TryActivate()
    {
        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;

        float distance = Vector3.Distance(transform.position, worldPos);

        if (distance <= activationRadius)
        {
            Debug.Log("Mouse in range");
            isHolding = true;
        }
        else
        {
            Debug.Log("Mouse out of range");
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (enableSquashStretch)
        {
            if (squashRoutine != null) StopCoroutine(squashRoutine);
            squashRoutine = StartCoroutine(SquashStretchPunch());
        }

        if (enableCameraShake && cam != null)
        {
            if (shakeRoutine != null) StopCoroutine(shakeRoutine);
            shakeRoutine = StartCoroutine(CameraShake());
        }
    }

    IEnumerator SquashStretchPunch()
    {
        // 3-step punch: squash -> stretch -> back
        float t1 = squashTotalTime * 0.33f;
        float t2 = squashTotalTime * 0.33f;
        float t3 = squashTotalTime * 0.34f;

        Vector3 squashScale = new Vector3(
            originalScale.x * stretchAmount,
            originalScale.y * squashAmount,
            originalScale.z
        );

        Vector3 stretchScale = new Vector3(
            originalScale.x * squashAmount,
            originalScale.y * stretchAmount,
            originalScale.z
        );

        yield return ScaleOverTime(transform, transform.localScale, squashScale, t1);
        yield return ScaleOverTime(transform, squashScale, stretchScale, t2);
        yield return ScaleOverTime(transform, stretchScale, originalScale, t3);

        transform.localScale = originalScale;
    }

    IEnumerator ScaleOverTime(Transform tr, Vector3 from, Vector3 to, float duration)
    {
        if (duration <= 0f)
        {
            tr.localScale = to;
            yield break;
        }

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.Clamp01(elapsed / duration);
            tr.localScale = Vector3.Lerp(from, to, k);
            yield return null;
        }
        tr.localScale = to;
    }

    IEnumerator CameraShake()
    {
        Transform camTr = cam.transform;
        Vector3 startPos = camTr.position;

        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            elapsed += Time.deltaTime;

            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            camTr.position = startPos + new Vector3(x, y, 0f);
            yield return null;
        }

        camTr.position = startPos;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}





