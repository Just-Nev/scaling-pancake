using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovementMobile : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float stopDistance = 0.5f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.2f;

    [Header("Cooldown UI")]
    public Image cooldownImage;
    public Vector3 uiOffset = new Vector3(0f, 1f, 0f);

    [Header("Squash & Stretch")]
    public float squashAmount = 0.8f;
    public float squashDuration = 0.07f;

    [Header("Sprite Flicker")]
    public Sprite defaultSprite;
    public Sprite rocketSprite;
    public float flickerSpeed = 0.1f;

    private SpriteRenderer sr;
    private Coroutine flickerRoutine;

    private float fireTimer = 0f;
    private Camera cam;
    private Vector3 targetPos;
    private bool isTouching = false;
    private bool isSquashing = false;
    private Vector3 originalScale;

    void Start()
    {
        cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        cooldownImage.fillAmount = 0f;
    }

    void Update()
    {
        HandleScrollWheel();  // Adjust stopDistance

        HandleUI();

        bool usedMobile = HandleMobileInput();
        bool usedController = HandleControllerInput();

        if (!usedMobile && !usedController)
        {
            StopFlicker();
            cooldownImage.fillAmount = 0f;
        }
    }


    // -------------------------
    // MOBILE INPUT
    // -------------------------
    bool HandleMobileInput()
    {
        isTouching = false;

        // Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                isTouching = true;
                Vector3 worldPos = cam.ScreenToWorldPoint(touch.position);
                worldPos.z = 0;
                targetPos = worldPos;
            }
        }
        // Mouse
        else if (Input.GetMouseButton(0))
        {
            isTouching = true;
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            targetPos = worldPos;
        }

        if (!isTouching) return false;

        // Movement
        Vector3 dir = targetPos - transform.position;
        float dist = dir.magnitude;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        if (dist > stopDistance)
        {
            transform.position += dir.normalized * moveSpeed * Time.deltaTime;

            if (flickerRoutine == null)
                flickerRoutine = StartCoroutine(Flicker());
        }
        else
        {
            StopFlicker();
        }

        // Shooting
        fireTimer += Time.deltaTime;
        cooldownImage.fillAmount = fireTimer / fireRate;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
            cooldownImage.fillAmount = 0f;
        }

        return true; // Used mobile input this frame
    }

    void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Positive or negative
        if (scroll != 0f)
        {
            stopDistance += scroll;      // Increase or decrease
            stopDistance = Mathf.Clamp(stopDistance, 0.1f, 5f); // Keep it within reasonable limits
        }
    }


    // -------------------------
    // CONTROLLER INPUT
    // -------------------------
    bool HandleControllerInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        float aimX = Input.GetAxis("RightStickHorizontal");
        float aimY = Input.GetAxis("RightStickVertical");

        bool moved = (moveX != 0 || moveY != 0);
        bool aimed = (aimX * aimX + aimY * aimY > 0.1f);

        // No controller activity?
        if (!moved && !aimed)
            return false;

        // Move
        if (moved)
        {
            Vector3 moveDir = new Vector3(moveX, moveY, 0f);
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            if (flickerRoutine == null)
                flickerRoutine = StartCoroutine(Flicker());
        }
        else
        {
            StopFlicker();
        }

        // Aim  shoot
        if (aimed)
        {
            float angle = Mathf.Atan2(aimY, aimX) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            fireTimer += Time.deltaTime;
            cooldownImage.fillAmount = fireTimer / fireRate;

            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
                cooldownImage.fillAmount = 0f;
            }
        }

        return true; // Used controller this frame
    }

    // -------------------------
    // UI
    // -------------------------
    void HandleUI()
    {
        cooldownImage.transform.position = transform.position + uiOffset;
        cooldownImage.transform.rotation = Quaternion.identity;
    }



    // -------------------------
    // SHOOTING EFFECTS
    // -------------------------
    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (!isSquashing)
            StartCoroutine(SquashStretch());
    }

    IEnumerator SquashStretch()
    {
        isSquashing = true;

        transform.localScale = new Vector3(
            originalScale.x,
            originalScale.y * squashAmount,
            originalScale.z
        );

        yield return new WaitForSeconds(squashDuration);

        transform.localScale = originalScale;
        isSquashing = false;
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            sr.sprite = rocketSprite;
            yield return new WaitForSeconds(flickerSpeed);
            sr.sprite = defaultSprite;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }

    void StopFlicker()
    {
        if (flickerRoutine != null)
        {
            StopCoroutine(flickerRoutine);
            flickerRoutine = null;
        }

        sr.sprite = defaultSprite;
    }
}


