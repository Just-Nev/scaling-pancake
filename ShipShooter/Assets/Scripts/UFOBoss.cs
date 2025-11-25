using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UFOBoss : MonoBehaviour
{
    [Header("Rotation")]
    public float rotationSpeed = 90f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public Transform[] firePoints;

    private float fireTimer;
    private bool isDead = false;

    [Header("Boss Health Bar")]
    public float maxHealth = 10f;
    public float health;
    public Image fillImage;
    public CanvasGroup healthBarUI;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float shakeIntensity = 6f;
    public float shakeTime = 0.1f;

    private void Start()
    {
        health = maxHealth;
        fillImage.fillAmount = 1f;
    }

    void Update()
    {
        if (isDead) return;

        RotateObject();
        HandleShooting();
    }

    void RotateObject()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
    }

    void HandleShooting()
    {
        if (firePoints.Length == 0 || bulletPrefab == null) return;

        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        foreach (Transform fp in firePoints)
        {
            if (fp != null)
                Instantiate(bulletPrefab, fp.position, fp.rotation);
        }
    }

    // Called by weak spots
    public void TakeDamageFromWeakSpot(float dmg)
    {
        if (isDead) return;

        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);

        StartCoroutine(FlashFill());
        StartCoroutine(ShakeHealthBar());
        StartCoroutine(SmoothHealthBar());

        if (health <= 0)
            Die();
    }

    IEnumerator FlashFill()
    {
        Color original = fillImage.color;
        fillImage.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        fillImage.color = original;
    }

    IEnumerator SmoothHealthBar()
    {
        float start = fillImage.fillAmount;
        float end = health / maxHealth;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * 5f;
            fillImage.fillAmount = Mathf.Lerp(start, end, t);
            yield return null;
        }
    }

    IEnumerator ShakeHealthBar()
    {
        Transform bar = healthBarUI.transform;
        Vector3 originalPos = bar.localPosition;

        float timer = 0f;
        while (timer < shakeTime)
        {
            float offset = Random.Range(-shakeIntensity, shakeIntensity);
            bar.localPosition = originalPos + new Vector3(offset, offset, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        bar.localPosition = originalPos;
    }

    void Die()
    {
        isDead = true;
        PolygonCollider2D col = GetComponent<PolygonCollider2D>();
        if (col != null) col.enabled = false;

        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // Fade the health bar
        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime;
            healthBarUI.alpha = t;
            yield return null;
        }

        healthBarUI.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    // Removes fire points when weak spot dies
    public void DisableFirePoints(Transform[] pointsToDisable)
    {
        List<Transform> newList = new List<Transform>();

        foreach (Transform fp in firePoints)
        {
            bool remove = false;

            foreach (Transform deadFP in pointsToDisable)
            {
                if (fp == deadFP)
                {
                    remove = true;
                    break;
                }
            }

            if (!remove)
                newList.Add(fp);
        }

        firePoints = newList.ToArray();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet") && !isDead)
        {
            Destroy(col.gameObject);

        }
    }
}


