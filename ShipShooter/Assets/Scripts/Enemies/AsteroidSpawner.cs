using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [System.Serializable]
    public class AsteroidEntry
    {
        public GameObject prefab;

        [Range(0f, 100f)]
        [Tooltip("Relative spawn chance. Does NOT need to add up to 100.")]
        public float spawnWeight = 1f;
    }

    [Header("Asteroids To Spawn (Weighted)")]
    public AsteroidEntry[] asteroids;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public float spawnDistance = 1f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
        InvokeRepeating(nameof(SpawnAsteroid), 1f, spawnInterval);
    }

    void SpawnAsteroid()
    {
        if (asteroids == null || asteroids.Length == 0)
        {
            Debug.LogWarning("AsteroidSpawner: No asteroids assigned!");
            return;
        }

        GameObject prefab = GetWeightedRandomPrefab();
        if (prefab == null) return;

        // ---- Pick a spawn position ----
        int side = Random.Range(0, 4);
        Vector3 spawnPos;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        switch (side)
        {
            case 0:
                spawnPos = new Vector3(Random.Range(-camWidth, camWidth), camHeight + spawnDistance, 0);
                break;
            case 1:
                spawnPos = new Vector3(Random.Range(-camWidth, camWidth), -camHeight - spawnDistance, 0);
                break;
            case 2:
                spawnPos = new Vector3(-camWidth - spawnDistance, Random.Range(-camHeight, camHeight), 0);
                break;
            default:
                spawnPos = new Vector3(camWidth + spawnDistance, Random.Range(-camHeight, camHeight), 0);
                break;
        }

        Instantiate(prefab, spawnPos, Quaternion.identity);
    }

    GameObject GetWeightedRandomPrefab()
    {
        float totalWeight = 0f;

        foreach (var entry in asteroids)
        {
            if (entry.prefab != null)
                totalWeight += Mathf.Max(0f, entry.spawnWeight);
        }

        if (totalWeight <= 0f)
            return null;

        float random = Random.Range(0f, totalWeight);

        foreach (var entry in asteroids)
        {
            if (entry.prefab == null) continue;

            random -= Mathf.Max(0f, entry.spawnWeight);
            if (random <= 0f)
                return entry.prefab;
        }

        return null;
    }
}





