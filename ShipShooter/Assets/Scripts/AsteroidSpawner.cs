using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Asteroids To Spawn")]
    public GameObject[] asteroidPrefabs;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnDistance = 1f;

    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    [Header("Random Size")]
    public float minScale = 0.8f;     // Slightly smaller
    public float maxScale = 1.3f;     // Slightly larger

    [Header("Random Rotation")]
    public float maxRotationSpeed = 120f; // degrees per second

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    void SpawnAsteroid()
    {
        if (asteroidPrefabs.Length == 0)
        {
            Debug.LogWarning("AsteroidSpawner: No asteroid prefabs assigned!");
            return;
        }

        GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

        // ------ Pick a spawn position ------
        int side = Random.Range(0, 4);
        Vector3 spawnPos = Vector3.zero;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        switch (side)
        {
            case 0: // Top
                spawnPos = new Vector3(Random.Range(-camWidth, camWidth), camHeight + spawnDistance, 0);
                break;
            case 1: // Bottom
                spawnPos = new Vector3(Random.Range(-camWidth, camWidth), -camHeight - spawnDistance, 0);
                break;
            case 2: // Left
                spawnPos = new Vector3(-camWidth - spawnDistance, Random.Range(-camHeight, camHeight), 0);
                break;
            case 3: // Right
                spawnPos = new Vector3(camWidth + spawnDistance, Random.Range(-camHeight, camHeight), 0);
                break;
        }

        // ------ Spawn asteroid ------
        GameObject asteroid = Instantiate(prefab, spawnPos, Quaternion.identity);

        // ------ Random Size ------
        float scale = Random.Range(minScale, maxScale);
        asteroid.transform.localScale = new Vector3(scale, scale, 1f);

        // ------ Move inward ------
        Vector3 direction = (Vector3.zero - spawnPos).normalized;
        float speed = Random.Range(minSpeed, maxSpeed);

        Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;

            // ------ Random rotation speed ------
            float rot = Random.Range(-maxRotationSpeed, maxRotationSpeed);
            rb.angularVelocity = rot;
        }
    }
}



