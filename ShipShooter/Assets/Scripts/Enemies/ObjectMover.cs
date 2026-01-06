using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ObjectMover : MonoBehaviour
{
    [Header("Movement")]
    public float minSpeed = 1f;
    public float maxSpeed = 3f;

    [Header("Random Size")]
    public float minScale = 0.8f;
    public float maxScale = 1.3f;

    [Header("Rotation")]
    public float maxRotationSpeed = 120f; // degrees per second

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        // ---- Random size ----
        float scale = Random.Range(minScale, maxScale);
        transform.localScale = new Vector3(scale, scale, 1f);

        // ---- Move toward center ----
        Vector2 direction = (-(Vector2)transform.position).normalized;
        float speed = Random.Range(minSpeed, maxSpeed);
        rb.linearVelocity = direction * speed;

        // ---- Random rotation ----
        rb.angularVelocity = Random.Range(-maxRotationSpeed, maxRotationSpeed);
    }
}

