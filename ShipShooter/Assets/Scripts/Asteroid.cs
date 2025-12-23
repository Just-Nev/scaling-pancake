using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Splitting")]
    public GameObject[] asteroidPrefabs;
    public int spawnCount = 3;
    public float spawnRadius = 1.2f;
    public float launchForce = 2f;

    [Header("Off Screen Cleanup")]
    public float offscreenLifetime = 3f; // seconds before asteroid auto-destroys
    private bool isOffscreen = false;
    private Coroutine offscreenRoutine;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    // === COLLISIONS ===
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            SplitAsteroid();
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Expl"))
        {
            Destroy(collision.gameObject);
            SplitAsteroid();
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Boss"))
        {
            SplitAsteroid();
            Destroy(gameObject);
        }

        if (col.gameObject.CompareTag("bBullet"))
        {
            Destroy(gameObject);
            SplitAsteroid();
            Destroy(col.gameObject);
        }


    }

    // === SPLITTING LOGIC ===
    void SplitAsteroid()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            // evenly spaced directions
            float angle = (360f / spawnCount) * i + Random.Range(-10f, 10f);
            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ).normalized;

            // spawn outside the asteroid
            Vector3 spawnPos = transform.position + (Vector3)(direction * spawnRadius);

            GameObject small = Instantiate(
                asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)],
                spawnPos,
                Quaternion.identity
            );

            // push outward
            Rigidbody2D rb = small.GetComponent<Rigidbody2D>();
            rb.AddForce(direction * launchForce, ForceMode2D.Impulse);
        }
    }

    // === OFFSCREEN CHECK (reliable) ===
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

    // Check if asteroid is outside camera view
    bool IsOffScreen()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        // Offscreen if outside the 0–1 viewport range
        return viewPos.x < 0f || viewPos.x > 1f ||
               viewPos.y < 0f || viewPos.y > 1f;
    }

    // Countdown for offscreen destruction
    IEnumerator OffscreenCountdown()
    {
        yield return new WaitForSeconds(offscreenLifetime);
        Destroy(gameObject);
    }
}





