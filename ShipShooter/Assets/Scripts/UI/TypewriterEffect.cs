using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float typingSpeed = 50f;

    [Header("Sound")]
    public AudioClip typingClip;
    public float minPitch = 0.95f;
    public float maxPitch = 1.05f;

    [Tooltip("Minimum time between typing sounds")]
    public float soundCooldown = 0.03f;

    private AudioSource audioSource;
    private float nextSoundTime;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f; // 2D
    }

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        // Make sure TMP has generated its textInfo
        textComponent.ForceMeshUpdate();

        int totalVisibleCharacters = textComponent.textInfo.characterCount;
        textComponent.maxVisibleCharacters = 0;

        while (textComponent.maxVisibleCharacters < totalVisibleCharacters)
        {
            textComponent.maxVisibleCharacters++;

            // Get the ACTUAL visible character (works with rich text tags)
            int visibleIndex = textComponent.maxVisibleCharacters - 1;
            var charInfo = textComponent.textInfo.characterInfo[visibleIndex];
            char c = charInfo.character;

            if (!char.IsWhiteSpace(c))
                PlayTypingSound();

            yield return new WaitForSeconds(1f / typingSpeed);
        }
    }

    void PlayTypingSound()
    {
        if (typingClip == null) return;

        // Rate-limit so sound keeps working at high speeds
        if (Time.unscaledTime < nextSoundTime) return;
        nextSoundTime = Time.unscaledTime + soundCooldown;

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.PlayOneShot(typingClip);
    }
}
