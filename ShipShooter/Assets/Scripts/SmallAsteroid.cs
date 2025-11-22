using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : MonoBehaviour
{
    [Header("Off Screen Cleanup")]
    public float offscreenLifetime = 3f;
    private bool isOffscreen = false;
    private Coroutine offscreenRoutine;
    private Camera cam;

    [Header("Particle")]
    public GameObject DesParticle;

    void Start()
    {
        cam = Camera.main;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Bullet destroys small asteroid
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Instantiate(DesParticle, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
 
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Boss"))
        {
            Instantiate(DesParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }

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

