using System.Collections;
using UnityEngine;

public class AsteroidStrong : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 3;
    private int currentHealth;

    [Header("Hit Effects")]
    public float squashAmount = 0.75f;     // how much it squashes
    public float squashDuration = 0.15f;   // how long squash lasts
    public float flashDuration = 0.1f;
    private SpriteRenderer sr;
    private bool isHitEffectRunning = false;

    [Header("Splitting")]
    public GameObject[] asteroidPrefabs;
    public int spawnCount = 3;
    public float spawnRadius = 1.2f;
    public float launchForce = 2f;

    [Header("Off Screen Cleanup")]
    public float offscreenLifetime = 3f;
    private bool isOffscreen = false;
    private Coroutine offscreenRoutine;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    // === COLLISIONS ===
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Boss"))
        {
            TakeDamage(maxHealth);
        }
    }

    // === DAMAGE LOGIC ===
    void TakeDamage(int amount)
    {
        currentHealth -= amount;

        // start squash and flash
        if (!isHitEffectRunning)
            StartCoroutine(HitEffects());

        if (currentHealth <= 0)
        {
            SplitAsteroid();
            Destroy(gameObject);
        }
    }

    IEnumerator HitEffects()
    {
        isHitEffectRunning = true;

        // store original
        Vector3 originalScale = transform.localScale;
        Color originalColor = sr.color;

        // squash
        transform.localScale = new Vector3(
            originalScale.x * squashAmount,
            originalScale.y * 1.25f,
            1f
        );

        // flash
        sr.color = Color.white;

        yield return new WaitForSeconds(squashDuration);

        // revert squash
        transform.localScale = originalScale;

        yield return new WaitForSeconds(flashDuration);

        // revert color
        sr.color = originalColor;

        isHitEffectRunning = false;
    }

    // === SPLITTING ===
    void SplitAsteroid()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float angle = (360f / spawnCount) * i + Random.Range(-10f, 10f);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;

            Vector3 spawnPos = transform.position + (Vector3)(direction * spawnRadius);

            GameObject small = Instantiate(
                asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                spawnPos,
                Quaternion.identity
            );

            Rigidbody2D rb = small.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
        }
    }

    // === OFFSCREEN CHECK ===
    void Update()
    {
        if (IsOffScreen())
        {
            if (!isOffscreen)
            {
                isOffscreen = true;
                offscreenRoutine = StartCoroutine(OffscreenCountdown());
            }
        }
        else
        {
            if (isOffscreen)
            {
                isOffscreen = false;

                if (offscreenRoutine != null)
                    StopCoroutine(offscreenRoutine);
            }
        }
    }

    bool IsOffScreen()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);
        return viewPos.x < 0f || viewPos.x > 1f ||
               viewPos.y < 0f || viewPos.y > 1f;
    }

    IEnumerator OffscreenCountdown()
    {
        yield return new WaitForSeconds(offscreenLifetime);
        Destroy(gameObject);
    }
}

