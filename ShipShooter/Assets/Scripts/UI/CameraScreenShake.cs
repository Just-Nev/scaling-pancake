using UnityEngine;

public class CameraScreenShake : MonoBehaviour
{
    public float shakeMagnitude = 0.2f;
    public float shakeSpeed = 20f;

    private Vector3 originalPosition;
    private float noiseSeed;
    private bool isShaking = false;

    void Start()
    {
        originalPosition = transform.localPosition;
        noiseSeed = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (isShaking)
        {
            float x = (Mathf.PerlinNoise(noiseSeed, Time.time * shakeSpeed) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(Time.time * shakeSpeed, noiseSeed) - 0.5f) * 2f;

            transform.localPosition = originalPosition + new Vector3(x, y, 0) * shakeMagnitude;
        }
        else
        {
            transform.localPosition = originalPosition;
        }
    }

    public void StartShake()
    {
        isShaking = true;
    }

    public void StopShake()
    {
        isShaking = false;
    }
}
