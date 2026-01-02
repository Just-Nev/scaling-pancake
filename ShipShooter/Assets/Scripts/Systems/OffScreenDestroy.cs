using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OffScreenDestroy : MonoBehaviour
{
    [Header("Off Screen Cleanup")]
    public float offscreenLifetime = 3f; 
    private bool isOffscreen = false;
    private Coroutine offscreenRoutine;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
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

    // Check if asteroid is outside camera view
    bool IsOffScreen()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(transform.position);

        // Offscreen if outside the 0–1 viewport range
        return viewPos.x < 0f || viewPos.x > 1f ||
               viewPos.y < 0f || viewPos.y > 1f;
    }

    // Countdown for offscreen destruction
    IEnumerator OffscreenCountdown()
    {
        yield return new WaitForSeconds(offscreenLifetime);
        Destroy(gameObject);
    }

}
