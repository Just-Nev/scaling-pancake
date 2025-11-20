using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpanwer : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public Vector2 spawnBoxSize = new Vector2(10f, 10f);
    public float spawnRate = 2f;
    public float spawnRateIncrement = 0.1f;
    public float maxSpawnRate = 0.5f;
    private float spawnTimer;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    private void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnRate)
        {
            SpawnAsteroid();
            spawnTimer = 0f;

            // Increase the spawn rate
            spawnRate -= spawnRateIncrement;
            spawnRate = Mathf.Max(spawnRate, maxSpawnRate);
        }
    }

    private void SpawnAsteroid()
    {
        // Generate a random point within the spawn box
        Vector2 randomPoint = new Vector2(
            Random.Range(-spawnBoxSize.x / 2f, spawnBoxSize.x / 2f),
            Random.Range(-spawnBoxSize.y / 2f, spawnBoxSize.y / 2f)
        );
        Vector3 spawnPosition = new Vector3(randomPoint.x, randomPoint.y, 0f) + transform.position;

        // Check for collisions with any object having the "Player" tag or "SmallAst"
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPosition, asteroidPrefab.transform.localScale.x);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return; // Retry spawning in the next frame
            }

            if (collider.CompareTag("SmallAst"))
            {
                return; // Retry spawning in the next frame
            }

            if (collider.CompareTag("Asteroid"))
            {
                return; // Retry spawning in the next frame
            }
        }

        // Check if the spawn position is too close to any other spawned asteroids
        // foreach (Vector3 position in spawnedPositions)
        // {
        //     if (Vector3.Distance(spawnPosition, position) < asteroidPrefab.transform.localScale.x)
        //     {
        //         return; // Retry spawning in the next frame
        //     }
        // }

        // Spawn the asteroid and give it a random initial direction
        GameObject asteroid = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
        Rigidbody2D asteroidRigidbody = asteroid.GetComponent<Rigidbody2D>();
        Vector2 randomDirection = Random.insideUnitCircle;
        asteroidRigidbody.AddForce(randomDirection.normalized * asteroidRigidbody.mass, ForceMode2D.Impulse);

        // Add the spawn position to the list of spawned positions
        spawnedPositions.Add(spawnPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnBoxSize.x, spawnBoxSize.y, 0f));
    }

}