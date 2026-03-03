using UnityEngine;

public class UpDownMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float maxUpOffset = 1f;     // How high above start position
    public float maxDownOffset = 1f;   // How far below start position

    [Header("Toggle Object Settings")]
    public GameObject targetObject;
    public float toggleInterval = 2f;

    private Vector3 startPosition;
    private bool movingDown = true;
    private float toggleTimer;

    void Start()
    {
        startPosition = transform.position;

        if (targetObject != null)
            targetObject.SetActive(true);
    }

    void Update()
    {
        MoveObject();
        HandleToggle();
    }

    void MoveObject()
    {
        float direction = movingDown ? -1f : 1f;
        transform.position += Vector3.up * direction * moveSpeed * Time.deltaTime;

        float upperLimit = startPosition.y + maxUpOffset;
        float lowerLimit = startPosition.y - maxDownOffset;

        if (transform.position.y <= lowerLimit)
        {
            transform.position = new Vector3(transform.position.x, lowerLimit, transform.position.z);
            movingDown = false; // Start moving up
        }
        else if (transform.position.y >= upperLimit)
        {
            transform.position = new Vector3(transform.position.x, upperLimit, transform.position.z);
            movingDown = true; // Start moving down
        }
    }

    void HandleToggle()
    {
        if (targetObject == null) return;

        toggleTimer += Time.deltaTime;

        if (toggleTimer >= toggleInterval)
        {
            targetObject.SetActive(!targetObject.activeSelf);
            toggleTimer = 0f;
        }
    }
}
