using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public float lifetime = 5f; // Lifetime of the ShockWave in seconds

    private float countdownTimer;

    private void Start()
    {
        countdownTimer = lifetime; // Initialize the timer with the lifetime value
    }

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        countdownTimer -= Time.deltaTime; // Decrement the timer by the elapsed time

        if (countdownTimer <= 0f)
        {
            DestroyShock(); // Call the method to destroy the shock wave
        }
    }

    private void DestroyShock()
    {
        Debug.Log("ShockWaveComplete");
        Destroy(gameObject); // Destroy the GameObject
    }
}
