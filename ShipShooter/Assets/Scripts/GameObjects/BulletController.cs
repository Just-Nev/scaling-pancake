using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    float maxX = 9.66f;
    float minX = -9.6f;
    float maxY = 6.37f;
    float minY = -4.45f;
    public float lifetime = 5f; // Lifetime of the bullet in seconds

    private Vector3 direction;
    private float countdownTimer;

    public void SetDirection(Vector3 direction)
    {
        this.direction = direction;
    }

    private void Start()
    {
        countdownTimer = lifetime; // Initialize the timer with the lifetime value
    }

    private void Update()
    {
        MoveBullet();
        WrapAround();
        UpdateTimer();
    }

    private void MoveBullet()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void WrapAround()
    {
        Vector3 currentPosition = transform.position;

        if (currentPosition.x > maxX)
        {
            currentPosition.x = minX;
        }
        else if (currentPosition.x < minX)
        {
            currentPosition.x = maxX;
        }

        if (currentPosition.y > maxY)
        {
            currentPosition.y = minY;
        }
        else if (currentPosition.y < minY)
        {
            currentPosition.y = maxY;
        }

        transform.position = currentPosition;
    }

    private void UpdateTimer()
    {
        // countdownTimer -= Time.deltaTime; // Decrement the timer by the elapsed time

        // if (countdownTimer <= 0f)
        // {
        //     DestroyBullet(); // Call the method to destroy the bullet
        // }
    }

    // private void DestroyBullet()
    // {
    //     Destroy(gameObject); // Destroy the bullet GameObject
    // }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw the bounds using Gizmos
        Gizmos.DrawLine(new Vector3(minX, minY, 0f), new Vector3(maxX, minY, 0f));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0f), new Vector3(maxX, maxY, 0f));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0f), new Vector3(minX, maxY, 0f));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0f), new Vector3(minX, minY, 0f));
    }
}
