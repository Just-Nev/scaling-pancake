using UnityEngine;

public enum MoveAxis
{
    X,
    Y
}

public class UpDownMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float maxUpOffset = 1f;
    public float maxDownOffset = 1f;

    [Header("Movement Axis")]
    public MoveAxis moveAxis = MoveAxis.Y;

    [Header("Main Object Settings")]
    public GameObject targetObject;
    public float toggleInterval = 2f;

    [Header("Prewarn Settings")]
    public GameObject preWarnObject;
    public float preWarnTime = 1f;

    private Vector3 startPosition;
    private bool movingDown = true;
    private float toggleTimer;
    private bool preWarnShown = false;

    void Start()
    {
        startPosition = transform.position;

        if (targetObject != null)
            targetObject.SetActive(false);

        if (preWarnObject != null)
            preWarnObject.SetActive(false);
    }

    void Update()
    {
        MoveObject();
        HandleToggle();
    }

    void MoveObject()
    {
        float direction = movingDown ? -1f : 1f;
        Vector3 moveVector = Vector3.zero;

        if (moveAxis == MoveAxis.Y)
            moveVector = Vector3.up * direction;
        else
            moveVector = Vector3.right * direction;

        transform.position += moveVector * moveSpeed * Time.deltaTime;

        float upperLimit;
        float lowerLimit;
        float currentPos;

        if (moveAxis == MoveAxis.Y)
        {
            currentPos = transform.position.y;
            upperLimit = startPosition.y + maxUpOffset;
            lowerLimit = startPosition.y - maxDownOffset;
        }
        else
        {
            currentPos = transform.position.x;
            upperLimit = startPosition.x + maxUpOffset;
            lowerLimit = startPosition.x - maxDownOffset;
        }

        if (currentPos <= lowerLimit)
        {
            SetPosition(lowerLimit);
            movingDown = false;
        }
        else if (currentPos >= upperLimit)
        {
            SetPosition(upperLimit);
            movingDown = true;
        }
    }

    void SetPosition(float value)
    {
        if (moveAxis == MoveAxis.Y)
            transform.position = new Vector3(transform.position.x, value, transform.position.z);
        else
            transform.position = new Vector3(value, transform.position.y, transform.position.z);
    }

    void HandleToggle()
    {
        if (targetObject == null) return;

        toggleTimer += Time.deltaTime;

        bool mainIsCurrentlyOn = targetObject.activeSelf;

        // Only show prewarn before turning the main object ON
        if (!mainIsCurrentlyOn)
        {
            if (!preWarnShown && toggleTimer >= (toggleInterval - preWarnTime))
            {
                if (preWarnObject != null)
                    preWarnObject.SetActive(true);

                preWarnShown = true;
            }
        }

        if (toggleTimer >= toggleInterval)
        {
            if (!mainIsCurrentlyOn)
            {
                // Turn main ON, hide prewarn
                targetObject.SetActive(true);

                if (preWarnObject != null)
                    preWarnObject.SetActive(false);
            }
            else
            {
                // Turn main OFF, no prewarn
                targetObject.SetActive(false);

                if (preWarnObject != null)
                    preWarnObject.SetActive(false);
            }

            toggleTimer = 0f;
            preWarnShown = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Bullet")) return;

        ResetAndDisable();

        // Optional: destroy bullet on hit
        Destroy(other.gameObject);
    }

    void ResetAndDisable()
    {
        toggleTimer = 0f;
        preWarnShown = false;

        if (targetObject != null)
            targetObject.SetActive(false);

        if (preWarnObject != null)
            preWarnObject.SetActive(false);
    }
}
