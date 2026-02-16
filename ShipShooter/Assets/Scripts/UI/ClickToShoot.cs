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
    public float squashAmount = 0.8f;
    public float squashDuration = 0.07f;

    private float fireTimer;
    private bool isHolding;
    private bool isSquashing;
    private Vector3 originalScale;
    private Camera cam;

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
        {
            TryActivate();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
        }

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

        if (!isSquashing)
            StartCoroutine(SquashStretch());
    }

    IEnumerator SquashStretch()
    {
        isSquashing = true;

        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * squashAmount,
            originalScale.z
        );

        yield return new WaitForSeconds(squashDuration);

        transform.localScale = originalScale;
        isSquashing = false;
    }

    // Draw activation radius in Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationRadius);
    }
}




