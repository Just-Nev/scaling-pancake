using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldownDuration = 0.5f; // Cooldown duration in seconds
    public AudioSource shootSnd;
    private float cooldownTimer = 0f;
    ScoreManager ScoreManager;
    public CooldownCircleController cooldownTimerUI;
    float bulletTime = 0.5f;

    // New public variables for squash and stretch customization
    public Vector3 squashScale = new Vector3(1.2f, 0.8f, 1f);
    public float squashDuration = 0.1f;

    private SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        GameObject scoreManager = GameObject.Find("ScoreManager");
        ScoreManager = scoreManager.GetComponent<ScoreManager>();

        playerSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (ScoreManager.ShootSpeed == 1)
        {
            cooldownDuration = 0.85f;
        }

        if (ScoreManager.ShootSpeed == 2)
        {
            cooldownDuration = 0.75f;
        }

        if (ScoreManager.ShootSpeed == 3)
        {
            cooldownDuration = 0.5f;
        }

        UpdateCooldown();

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit") || Input.GetAxis("RT") > 0.5f) && cooldownTimer <= 0f)
        {
            FireBullet();
            cooldownTimer = cooldownDuration; // Reset the cooldown timer
        }
    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        cooldownTimerUI.SetCooldown(cooldownTimer, cooldownDuration);
    }

    private IEnumerator SquashAndStretch()
    {
        Vector3 initialScale = transform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < squashDuration)
        {
            float t = elapsedTime / squashDuration;
            transform.localScale = Vector3.Lerp(initialScale, squashScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;
    }

    private void FireBullet()
    {
        if (ScoreManager.shootRange == 1)
        {
            bulletTime = 0.65f;
        }

        if (ScoreManager.shootRange == 2)
        {
            bulletTime = 0.85f;
        }

        if (ScoreManager.shootRange == 3)
        {
            bulletTime = 1.05f;
        }

        StartCoroutine(SquashAndStretch());

        Vector3 spawnPosition = transform.position + transform.up * 1f;
        Quaternion spawnRotation = transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        BulletController bulletController = bullet.GetComponent<BulletController>();
        bulletController.SetDirection(transform.up);
        shootSnd.Play();
        Destroy(bullet, bulletTime);
    }
}
