using UnityEngine;

public class OnPressMoveUp : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxHeight = 10f;   // Optional height limit (set high if not needed)

    private bool isMovingUp = false;
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        if (isMovingUp)
        {
            Vector3 newPos = transform.position + Vector3.up * moveSpeed * Time.deltaTime;

            // Optional height limit
            if (newPos.y <= startY + maxHeight)
            {
                transform.position = newPos;
            }
        }
    }

    // Called when button is pressed down
    public void StartMoveUp()
    {
        isMovingUp = true;
    }

    // Called when button is released
    public void StopMoveUp()
    {
        isMovingUp = false;
    }
}
