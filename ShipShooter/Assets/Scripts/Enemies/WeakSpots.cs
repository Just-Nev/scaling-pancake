using UnityEngine;
using System.Collections;

public class WeakSpots : MonoBehaviour
{
    [Header("Weak Spot Settings")]
    public UFOBoss boss;
    public float damageToBoss = 1f;
    public float maxHealth = 3f;
    private float currentHealth;

    [Header("Fire Points Assigned to This Weak Spot")]
    public Transform[] firePoints;

    [Header("Squash & Stretch")]
    public float squashAmount = 0.7f;
    public float squashDuration = 0.07f;

    private Vector3 originalScale;
    private bool isSquashing = false;

    private void Start()
    {
        currentHealth = maxHealth;
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet"))
        {
            Destroy(col.gameObject);

            if (boss != null)
                boss.TakeDamageFromWeakSpot(damageToBoss);

            TakeDamage();

            if (!isSquashing)
                StartCoroutine(Squash());
        }
    }

    void TakeDamage()
    {
        currentHealth -= 1f;

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator Squash()
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

    void Die()
    {
        // Disable visuals + collider
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;

        // Tell the boss to stop firing from these fire points
        if (boss != null && firePoints.Length > 0)
            boss.DisableFirePoints(firePoints);

        // Object stays alive so firing doesn’t break
    }
}



