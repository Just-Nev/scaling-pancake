using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;

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
    private Vector3 originalScale;
    private bool isSquashing = false;

    private float fireTimer = 0f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
        cooldownImage.fillAmount = 0f;
    }

    void Update()
    {
        HandleUI();
        HandleMovement();
        HandleAimAndShoot();
    }

    void HandleUI()
    {
        cooldownImage.transform.position = transform.position + uiOffset;
        cooldownImage.transform.rotation = Quaternion.identity;
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(moveX, moveY, 0f);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            transform.position += moveDir.normalized * moveSpeed * Time.deltaTime;

            if (flickerRoutine == null)
                flickerRoutine = StartCoroutine(Flicker());
        }
        else
        {
            StopFlicker();
        }
    }

    void HandleAimAndShoot()
    {
        float aimX = Input.GetAxis("RightStickHorizontal");
        float aimY = Input.GetAxis("RightStickVertical");

        Vector3 aimDir = new Vector3(aimX, aimY, 0f);

        if (aimDir.sqrMagnitude > 0.1f)
        {
            // Rotate player to aim direction
            float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            // Fire timer
            fireTimer += Time.deltaTime;
            cooldownImage.fillAmount = fireTimer / fireRate;

            if (fireTimer >= fireRate)
            {
                Shoot();
                fireTimer = 0f;
                cooldownImage.fillAmount = 0f;
            }
        }
        else
        {
            cooldownImage.fillAmount = 0f;
        }
    }

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


