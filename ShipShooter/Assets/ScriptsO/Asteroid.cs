using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public GameObject smallerAsteroidPrefab; // prefab for smaller asteroids
    public int splitCount = 3;               // number of smaller asteroids to spawn
    public float splitSpeed = 2f;            // speed of the smaller asteroids
    public float spawnOffset = 0.5f;         // distance from center to spawn smaller asteroids

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            SplitAsteroid();
            Destroy(collision.gameObject); // destroy the bullet
            Destroy(gameObject);           // destroy this asteroid
        }
    }

    void SplitAsteroid()
    {
        if (smallerAsteroidPrefab == null) return;

        for (int i = 0; i < splitCount; i++)
        {
            // Random direction to offset spawn so they don't overlap
            float angle = Random.Range(0f, 360f);
            Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * spawnOffset;

            // Spawn the smaller asteroid slightly away from center
            GameObject small = Instantiate(smallerAsteroidPrefab, (Vector2)transform.position + offset, Quaternion.identity);

            // Give it a random velocity in a random direction
            Rigidbody2D smallRb = small.GetComponent<Rigidbody2D>();
            if (smallRb != null)
            {
                float moveAngle = Random.Range(0f, 360f);
                Vector2 dir = new Vector2(Mathf.Cos(moveAngle * Mathf.Deg2Rad), Mathf.Sin(moveAngle * Mathf.Deg2Rad));
                smallRb.linearVelocity = dir.normalized * splitSpeed;
            }
        }
    }
}


