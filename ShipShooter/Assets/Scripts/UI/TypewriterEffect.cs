using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float typingSpeed = 50f;

    void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.ForceMeshUpdate();
        int totalVisibleCharacters = textComponent.textInfo.characterCount;

        textComponent.maxVisibleCharacters = 0;

        while (textComponent.maxVisibleCharacters < totalVisibleCharacters)
        {
            textComponent.maxVisibleCharacters++;
            yield return new WaitForSeconds(1f / typingSpeed);
        }
    }
}
