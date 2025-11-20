using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public float spawnInterval = 1.5f;
    public float spawnDistance = 1f;   // How far off-screen to spawn
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    void SpawnAsteroid()
    {
        // Randomly pick one of 4 sides of the screen
        int side = Random.Range(0, 4);

        Vector3 spawnPos = Vector3.zero;

        // Get screen bounds in world units
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        switch (side)
        {
            case 0: // Top
                spawnPos = new Vector3(
                    Random.Range(-camWidth, camWidth),
                    camHeight + spawnDistance,
                    0);
                break;

            case 1: // Bottom
                spawnPos = new Vector3(
                    Random.Range(-camWidth, camWidth),
                    -camHeight - spawnDistance,
                    0);
                break;

            case 2: // Left
                spawnPos = new Vector3(
                    -camWidth - spawnDistance,
                    Random.Range(-camHeight, camHeight),
                    0);
                break;

            case 3: // Right
                spawnPos = new Vector3(
                    camWidth + spawnDistance,
                    Random.Range(-camHeight, camHeight),
                    0);
                break;
        }

        // Spawn asteroid
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);

        // Move the asteroid toward the center of the screen
        Vector3 target = Vector3.zero;   // center
        Vector3 direction = (target - spawnPos).normalized;

        float speed = Random.Range(minSpeed, maxSpeed);

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
}

