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
    public Vector3 uiOffset = new Vector3(0f, 50f, 0f); // Pixels above player

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
        HandleScrollWheel();

        bool usedMobile = HandleMobileInput();
        bool usedController = HandleControllerInput();

        if (!usedMobile && !usedController)
        {
            StopFlicker();
            cooldownImage.fillAmount = 0f;
        }
    }

    void LateUpdate()
    {
        if (cooldownImage != null)
        {
            Vector3 worldPos = transform.position; // Player world position
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(worldPos);

            // Convert screen point to canvas space
            Vector2 canvasPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cooldownImage.canvas.transform as RectTransform,
                screenPoint,
                cooldownImage.canvas.worldCamera,
                out canvasPos
            );

            // Set position with offset
            cooldownImage.rectTransform.anchoredPosition = canvasPos + new Vector2(uiOffset.x, uiOffset.y + 80f);
        }
    }


    // -------------------------
    // SCROLL WHEEL TO ADJUST STOP DISTANCE
    // -------------------------
    void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            stopDistance += scroll;
            stopDistance = Mathf.Clamp(stopDistance, 1f, 5f);
        }
    }

    // -------------------------
    // MOBILE INPUT
    // -------------------------
    bool HandleMobileInput()
    {
        isTouching = false;

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
        else if (Input.GetMouseButton(0))
        {
            isTouching = true;
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            worldPos.z = 0;
            targetPos = worldPos;
        }

        if (!isTouching) return false;

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

        fireTimer += Time.deltaTime;
        cooldownImage.fillAmount = fireTimer / fireRate;

        if (fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;
            cooldownImage.fillAmount = 0f;
        }

        return true;
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

        if (!moved && !aimed)
            return false;

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

        return true;
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





