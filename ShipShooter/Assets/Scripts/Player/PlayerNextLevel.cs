using System.Collections;
using UnityEngine;

public class PlayerNextLevel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovementMobile playerMovement;

    [Header("Center + Face Up")]
    [SerializeField] private float moveToCenterSpeed = 5f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float arriveDistance = 0.05f;
    [SerializeField] private float faceUpZRotation = 0f;

    [Header("Launch Sequence (Button Activated)")]
    [Tooltip("How far the ship dips down before launch.")]
    public float dipDownDistance = 0.35f;
    public GameObject shipTrail;

    [Tooltip("Speed of the dip down movement.")]
    public float dipDownSpeed = 3f;

    [Tooltip("Pause time after dipping down.")]
    public float dipPauseTime = 0.25f;

    [Tooltip("Initial upward speed.")]
    public float flyUpSpeed = 8f;

    [Tooltip("Optional acceleration while flying up (0 = constant speed).")]
    public float flyUpAcceleration = 0f;

    [Tooltip("How long to fly up. Set 0 for infinite until stopped.")]
    public float flyUpDuration = 2f;

    public bool useUnscaledTime = false;

    private bool movingToCenter = false;
    private Vector3 targetCenter;
    private bool readyForButton = false;

    private Coroutine launchRoutine;

    void Update()
    {
        if (movingToCenter)
            MoveAndRotateToCenter();
    }

    public void BeginSequenceToCenter()
    {
        if (movingToCenter || readyForButton) return;

        if (playerMovement != null)
            playerMovement.LockControls();

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // swap to rb.velocity if your project uses that
            rb.angularVelocity = 0f;
        }

        Camera cam = Camera.main;
        Vector3 centre = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, cam.nearClipPlane));
        centre.z = transform.position.z;

        targetCenter = centre;
        movingToCenter = true;
        readyForButton = false;
    }

    private void MoveAndRotateToCenter()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetCenter,
            moveToCenterSpeed * Time.deltaTime
        );

        Quaternion targetRot = Quaternion.Euler(0f, 0f, faceUpZRotation);
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRot,
            rotationSpeed * Time.deltaTime
        );

        bool posDone = Vector3.Distance(transform.position, targetCenter) <= arriveDistance;
        bool rotDone = Quaternion.Angle(transform.rotation, targetRot) <= 1f;

        if (posDone && rotDone)
        {
            transform.position = targetCenter;
            transform.rotation = targetRot;

            movingToCenter = false;
            readyForButton = true;
        }
    }

    /// <summary>
    /// Hook this to a UI Button OnClick.
    /// Does: dip down -> pause -> fly up
    /// </summary>
    public void LaunchButtonPressed()
    {
        if (!readyForButton) return;

        

        if (launchRoutine != null)
            StopCoroutine(launchRoutine);

        launchRoutine = StartCoroutine(LaunchSequenceRoutine());
    }

    public void StopLaunch()
    {
        if (launchRoutine != null)
        {
            StopCoroutine(launchRoutine);
            launchRoutine = null;
        }
    }

    private IEnumerator LaunchSequenceRoutine()
    {
        // DIP DOWN
        Vector3 startPos = transform.position;
        Vector3 dipTarget = startPos + Vector3.down * dipDownDistance;

        while (Vector3.Distance(transform.position, dipTarget) > 0.001f)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            transform.position = Vector3.MoveTowards(
                transform.position,
                dipTarget,
                dipDownSpeed * dt
            );

            yield return null;
        }

        transform.position = dipTarget;

        // PAUSE
        if (dipPauseTime > 0f)
        {
            if (useUnscaledTime)
            {
                float t = 0f;
                while (t < dipPauseTime) { t += Time.unscaledDeltaTime; yield return null; }
            }
            else
            {
                yield return new WaitForSeconds(dipPauseTime);
            }
        }

        // FLY UP
        float elapsed = 0f;
        float currentSpeed = flyUpSpeed;

        while (flyUpDuration <= 0f || elapsed < flyUpDuration)
        {
            shipTrail.SetActive(true);

            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            if (flyUpAcceleration != 0f)
                currentSpeed += flyUpAcceleration * dt;

            transform.position += Vector3.up * currentSpeed * dt;

            elapsed += dt;
            yield return null;
        }

        launchRoutine = null;
    }

    public bool IsReadyForButton() => readyForButton;
}
