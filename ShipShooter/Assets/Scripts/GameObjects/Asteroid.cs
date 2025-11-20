using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public GameObject[] asteroidPrefabs;
    public int spawnCount = 3;
    public float spawnRadius = 1f;
    public GameObject explosion;
    public GameObject explosiveEffect;
    public AudioSource exp;


    void Start(){
        
        explosion = GameObject.Find("explosion");
        exp = explosion.GetComponent<AudioSource>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") )
        {
            exp.Play();
            // Instantiate the death particle effect
            //Instantiate(explosiveEffect, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f) * spawnRadius;
                GameObject spawnedAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawnPosition, Quaternion.identity);
                Rigidbody2D spawnedRigidbody = spawnedAsteroid.GetComponent<Rigidbody2D>();
                spawnedRigidbody.AddForce(randomDirection * spawnedRigidbody.mass, ForceMode2D.Impulse);
            }

            Destroy(gameObject);

            GameObject scoreManagerObj = GameObject.Find("ScoreManager");
            ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
            scoreManager.IncrementScoreBigAsteroid();
        }

        if (collision.gameObject.CompareTag("ShockWave") )
        {
            exp.Play();

            //Destroy(collision.gameObject);

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f) * spawnRadius;
                GameObject spawnedAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawnPosition, Quaternion.identity);
                Rigidbody2D spawnedRigidbody = spawnedAsteroid.GetComponent<Rigidbody2D>();
                spawnedRigidbody.AddForce(randomDirection * spawnedRigidbody.mass, ForceMode2D.Impulse);
            }

            Destroy(gameObject);

            GameObject scoreManagerObj = GameObject.Find("ScoreManager");
            ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
            scoreManager.IncrementScoreBigAsteroid();
        }

        if (collision.gameObject.CompareTag("Boss1"))
        {
            exp.Play();

            //Destroy(collision.gameObject);

            for (int i = 0; i < spawnCount; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle.normalized;
                Vector3 spawnPosition = transform.position + new Vector3(randomDirection.x, randomDirection.y, 0f) * spawnRadius;
                GameObject spawnedAsteroid = Instantiate(asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)], spawnPosition, Quaternion.identity);
                Rigidbody2D spawnedRigidbody = spawnedAsteroid.GetComponent<Rigidbody2D>();
                spawnedRigidbody.AddForce(randomDirection * spawnedRigidbody.mass, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
        }

    }
}
