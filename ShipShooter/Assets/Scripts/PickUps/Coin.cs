using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Timing")]
    public float timeBeforeFlash = 2f;     // Time before it starts flashing
    public float flashDuration = 2f;       // How long it flashes before being destroyed
    public float flashSpeed = 8f;          // Speed of fade in/out

    private SpriteRenderer sr;
    private float timer;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Start flashing after the delay
        if (timer > timeBeforeFlash)
        {
            float flashTime = timer - timeBeforeFlash;

            // Flashing effect using sine wave for smooth fade
            float alpha = Mathf.Abs(Mathf.Sin(flashTime * flashSpeed));
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

            // Destroy after flashing for the given duration
            if (flashTime >= flashDuration)
            {
                Destroy(gameObject);
            }
        }
    }
}
