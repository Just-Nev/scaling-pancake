using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    public float destroyDelay = 2f; // Customize the destroy delay here

    private void Start()
    {
        // Destroy the game object after the specified delay
        Destroy(gameObject, destroyDelay);
    }
}
