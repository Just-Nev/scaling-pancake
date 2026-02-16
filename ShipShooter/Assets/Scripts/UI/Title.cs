using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour
{
    [Header("Flash Settings")]
    public Color flashColor = Color.red;
    public float flashDuration = 0.1f;

    private SpriteRenderer sr;
    private Color originalColor;
    private Coroutine flashRoutine;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            TriggerFlash();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(other.gameObject);
            TriggerFlash();
        }
    }

    void TriggerFlash()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = originalColor;
    }
}






