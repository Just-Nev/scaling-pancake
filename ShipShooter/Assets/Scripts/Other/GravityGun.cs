using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    public Transform gunTip; // The end of the gun barrel
    public float maxDistance = 5f;
    public float throwForce = 10f;
    public float liftForce = 5f;
    public float maxLiftDistance = 2f;
    public float rotationSpeed = 5f;

    private GameObject currentObject;
    private Rigidbody2D currentRigidbody;
    private bool isLiftingObject = false;

private void FixedUpdate()
{
    RotateGun();

    if (Input.GetMouseButtonDown(0))
    {
        if (currentObject == null)
        {
            TryPickupObject();
        }
        else
        {
            if (isLiftingObject)
            {
                ReleaseObject();
            }
            else
            {
                ThrowObject();
            }
        }
    }
    else if (Input.GetMouseButtonDown(1))
    {
        if (isLiftingObject)
        {
            ThrowObject();
        }
    }

    if (isLiftingObject)
    {
        MoveObject();
    }
}

private void RotateGun()
{
    Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector3 direction = mousePosition - transform.position;
    direction.z = 0f;
    direction.Normalize();

    float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    Quaternion desiredRotation = Quaternion.Euler(0f, 0f, rotationZ);

    gunTip.rotation = Quaternion.Slerp(gunTip.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
}


    private void TryPickupObject()
    {
        RaycastHit2D hit = Physics2D.Raycast(gunTip.position, gunTip.right, maxDistance);
        if (hit.collider != null && hit.collider.CompareTag("Throwable"))
        {
            currentObject = hit.collider.gameObject;
            currentRigidbody = currentObject.GetComponent<Rigidbody2D>();
            currentRigidbody.gravityScale = 0f;
            currentRigidbody.velocity = Vector2.zero;
            //currentRigidbody.isKinematic = true; // Disable physics interactions

            isLiftingObject = true;
        }
    }

    private void ReleaseObject()
    {
        currentRigidbody.gravityScale = 1f;
        currentRigidbody.isKinematic = false; // Enable physics interactions

        currentObject = null;
        currentRigidbody = null;
        isLiftingObject = false;
    }

    private void ThrowObject()
    {
        currentRigidbody.gravityScale = 1f;
        currentRigidbody.isKinematic = false; // Enable physics interactions

        currentRigidbody.AddForce(gunTip.right * throwForce, ForceMode2D.Impulse);
        currentObject = null;
        currentRigidbody = null;
    }

    private void MoveObject()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 moveDirection = ((Vector2)mousePosition - (Vector2)gunTip.position).normalized;

        float distanceToMouse = Vector2.Distance(gunTip.position, mousePosition);
        float t = Mathf.Clamp01(distanceToMouse / maxLiftDistance);

        Vector2 targetPosition = (Vector2)gunTip.position + moveDirection * (maxLiftDistance * t);

        // Apply smoothing using Lerp
        float smoothingFactor = 1f; // Adjust this value to control the smoothness (0 = no smoothing, 1 = maximum smoothness)
        Vector2 smoothedPosition = Vector2.Lerp(currentRigidbody.position, targetPosition, smoothingFactor);

        currentRigidbody.MovePosition(smoothedPosition);
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(gunTip.position, gunTip.position + gunTip.right * maxDistance);
    }
}
