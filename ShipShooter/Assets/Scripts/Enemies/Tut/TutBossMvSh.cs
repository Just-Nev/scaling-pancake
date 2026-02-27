using UnityEngine;
using UnityEngine.UI;
using System.Collections; 

public class TutBossMvSh : MonoBehaviour
{
    [Header("VFX")]
    public GameObject HitParticle;
    public GameObject DeathParticle;

    [Header("Boss Health")]
    public float maxHealth = 10f;
    public float health;
    public Image fillImage;          // Assign your "Health Bar Fill"
    public CanvasGroup healthBarUI;

    [Header("Hit Feedback")]
    public float flashDuration = 0.1f;
    public float shakeIntensity = 6f;
    public float shakeTime = 0.1f;

    private Camera cam;
    private bool isDead = false;


    private void Start()
    {
        cam = Camera.main;

        health = maxHealth;
        fillImage.fillAmount = 1f;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet") && !isDead)
        {
            Destroy(col.gameObject);
            Instantiate(HitParticle, transform.position, Quaternion.identity);

            TakeDamage(1f);
        }
    }

    private void TakeDamage(float dmg)
    {
        health -= dmg;
        health = Mathf.Clamp(health, 0, maxHealth);

        StartCoroutine(FlashFill());
        StartCoroutine(ShakeHealthBar());
        StartCoroutine(SmoothHealthBar());

        if (health <= 0)
        {
            Die();
        }
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

    private void Die()
    {
        isDead = true;
        GetComponent<PolygonCollider2D>().enabled = false;
        Instantiate(DeathParticle, transform.position, Quaternion.identity);
        StartCoroutine(DeathSequence());
    }

    IEnumerator DeathSequence()
    {
        // Fade the health bar first
        yield return StartCoroutine(FadeHealthBar());

        // Now disable boss
        gameObject.SetActive(false);
    }

    IEnumerator FadeHealthBar()
    {
        float t = 1f;

        while (t > 0)
        {
            t -= Time.deltaTime;
            healthBarUI.alpha = t;
            yield return null;
        }

        healthBarUI.gameObject.SetActive(false);
    }
}
