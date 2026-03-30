using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceRoller : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image diceImage;
    [SerializeField] private Button rollButton;
    [SerializeField] private TextMeshProUGUI quanText;

    [Header("Dice Sprites")]
    [SerializeField] private Sprite[] diceSprites;

    [Header("Roll Settings")]
    [SerializeField] private float rollDuration = 1.0f;
    [SerializeField] private float flickerSpeed = 0.08f;

    public int rollsRemaining = 3;

    private Coroutine rollCoroutine;

    private void Awake()
    {
        rollButton.onClick.AddListener(RollDice);
    }

    private void Start()
    {
        // Sync from RunManager
        rollsRemaining = RunManager.Instance.currentReroll;

        UpdateQuantityText();
        UpdateButtonState();
    }

    private void OnDestroy()
    {
        rollButton.onClick.RemoveListener(RollDice);
    }

    public void RollDice()
    {
        if (diceSprites == null || diceSprites.Length == 0 || diceImage == null)
            return;

        if (rollsRemaining <= 0)
        {
            rollButton.interactable = false;
            return;
        }

        if (rollCoroutine != null)
            StopCoroutine(rollCoroutine);

        rollCoroutine = StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        rollButton.interactable = false;

        float timer = 0f;
        int lastIndex = -1;

        while (timer < rollDuration)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, diceSprites.Length);
            }
            while (diceSprites.Length > 1 && randomIndex == lastIndex);

            lastIndex = randomIndex;
            diceImage.sprite = diceSprites[randomIndex];

            yield return new WaitForSeconds(flickerSpeed);
            timer += flickerSpeed;
        }

        int finalIndex = Random.Range(0, diceSprites.Length);
        diceImage.sprite = diceSprites[finalIndex];

        // Decrease rolls and sync with RunManager
        rollsRemaining--;
        RunManager.Instance.currentReroll = rollsRemaining;

        UpdateQuantityText();
        UpdateButtonState();

        rollCoroutine = null;
    }

    private void UpdateQuantityText()
    {
        if (quanText != null)
        {
            quanText.text = rollsRemaining.ToString();

            var colors = rollButton.colors;
            quanText.color = rollsRemaining <= 0
                ? colors.disabledColor
                : colors.normalColor;
        }
    }

    private void UpdateButtonState()
    {
        rollButton.interactable = rollsRemaining > 0;
    }

    public void AddRolls(int amount)
    {
        rollsRemaining += amount;
        RunManager.Instance.currentReroll = rollsRemaining;

        UpdateQuantityText();
        UpdateButtonState();
    }
}
