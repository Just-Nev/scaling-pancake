using UnityEngine;

public class SimpleBoss : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 3f;
    public float moveDownAmount = 0.5f;

    [Header("Screen Wrap")]
    public float topLimit = 5f;
    public float bottomLimit = -5f;

    private bool movingRight = true;

    private Camera mainCamera;
    private float screenLeft;
    private float screenRight;

    private void Start()
    {
        mainCamera = Camera.main;
        float camHeight = mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        screenLeft = -camWidth + 0.5f;
        screenRight = camWidth - 0.5f;
    }

    private void FixedUpdate()
    {
        MoveHorizontal();
        CheckEdges();
        CheckWrap();
    }

    private void MoveHorizontal()
    {
        Vector3 moveDir = movingRight ? Vector3.right : Vector3.left;
        transform.Translate(moveDir * speed * Time.fixedDeltaTime);
    }

    private void CheckEdges()
    {
        if (movingRight && transform.position.x >= screenRight)
        {
            movingRight = false;
            MoveDown();
        }
        else if (!movingRight && transform.position.x <= screenLeft)
        {
            movingRight = true;
            MoveDown();
        }
    }

    private void MoveDown()
    {
        transform.Translate(Vector3.down * moveDownAmount);
    }

    private void CheckWrap()
    {
        if (transform.position.y < bottomLimit)
        {
            transform.position = new Vector3(transform.position.x, topLimit, transform.position.z);
        }
    }

}



