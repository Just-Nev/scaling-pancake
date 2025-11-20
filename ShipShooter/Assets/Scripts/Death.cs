using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Death : MonoBehaviour
{
    [Header("Explosion Effect")]
    public GameObject deathEffect;

    [Header("Screen Shake")]
    public Camera mainCamera;
    public float shakeDuration = 0.3f;
    public float shakeMagnitude = 0.2f;

    private Vector3 originalCamPos;

    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera != null)
            originalCamPos = mainCamera.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Die();
        }
    }

    void Die()
    {
        // Spawn explosion
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Disable player visuals and physics
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        // Start screen shake
        if (mainCamera != null)
            StartCoroutine(ScreenShake());

        // Optional: restart scene or handle game over
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator ScreenShake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.position = originalCamPos + new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = originalCamPos;
    }
}


