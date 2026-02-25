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
    public float squashAmount = 0.85f;
    public float stretchAmount = 1.15f;
    public float squashTotalTime = 0.12f;

    [Header("Sprite Flicker (Button Toggle)")]
    public bool enableSpriteFlicker = true;
    public Sprite originalSprite;
    public Sprite flickerSprite;
    public float flickerInterval = 0.08f;   // lower = faster flicker
    public bool startFlickerOnEnable = false;

    private float fireTimer;
    private bool isHolding;
    private Vector3 originalScale;
    private Camera cam;

    private Coroutine squashRoutine;

    private SpriteRenderer sr;
    private Coroutine flickerRoutine;
    private bool isFlickering;

    void Start()
    {
        cam = Camera.main;
        originalScale = transform.localScale;

        sr = GetComponent<SpriteRenderer>();
        if (sr != null && originalSprite == null)
            originalSprite = sr.sprite;

        if (cooldownImage != null)
            cooldownImage.fillAmount = 0f;

        if (startFlickerOnEnable)
            StartFlicker();
    }

    void Update()
    {
        // Your original mouse-hold shooting still works
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
        if (cam == null) return;

        Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        worldPos.z = 0f;

        float distance = Vector3.Distance(transform.position, worldPos);

        if (distance <= activationRadius)
            isHolding = true;
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (enableSquashStretch)
        {
            if (squashRoutine != null) StopCoroutine(squashRoutine);
            squashRoutine = StartCoroutine(SquashStretchPunch());
        }
    }

    // ---------- FLICKER CONTROL (Call from Button) ----------

    // Hook UI Button OnClick() to this
    public void ToggleFlicker()
    {
        if (isFlickering) StopFlicker();
        else StartFlicker();
    }

    public void StartFlicker()
    {
        if (!enableSpriteFlicker || sr == null || originalSprite == null || flickerSprite == null)
            return;

        if (flickerRoutine != null) StopCoroutine(flickerRoutine);
        isFlickering = true;
        flickerRoutine = StartCoroutine(FlickerLoop());
    }

    public void StopFlicker()
    {
        isFlickering = false;

        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        if (sr != null && originalSprite != null)
            sr.sprite = originalSprite;
    }

    IEnumerator FlickerLoop()
    {
        bool state = false;

        while (isFlickering)
        {
            state = !state;
            sr.sprite = state ? flickerSprite : originalSprite;
            yield return new WaitForSeconds(flickerInterval);
        }
    }

    // ---------- SQUASH / STRETCH ----------

    IEnumerator SquashStretchPunch()
    {
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

    void OnDisable()
    {
        // Safety: stop coroutine + restore sprite if object disabled
        StopFlicker();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}