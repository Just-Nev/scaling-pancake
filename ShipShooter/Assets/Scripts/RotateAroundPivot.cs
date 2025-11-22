using UnityEngine;

public class OrbitAroundPivot : MonoBehaviour
{
    [Header("Pivot Settings")]
    public Transform pivot;       // Object to orbit around
    public float speed = 50f;     // Degrees per second

    private Vector3 offset;       // Distance from pivot (radius)

    void Start()
    {
        if (pivot == null)
        {
            Debug.LogError("Pivot not assigned! Please assign a pivot object.");
            enabled = false;
            return;
        }

        // Store the initial offset (radius) from the pivot
        offset = transform.position - pivot.position;
    }

    void Update()
    {
        if (pivot == null) return;

        // Rotate the offset vector around Z-axis
        float angle = speed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector3 newOffset = new Vector3(
            offset.x * cos - offset.y * sin,
            offset.x * sin + offset.y * cos,
            0f
        );

        // Update position relative to the pivot
        transform.position = pivot.position + newOffset;

        // Save the rotated offset for the next frame
        offset = newOffset;
    }
}




