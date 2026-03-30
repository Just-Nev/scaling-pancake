using UnityEngine;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI MoneyText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MoneyText.text = RunManager.Instance.currentMoney.ToString();
    }

    public void upText()
    {
        MoneyText.text = RunManager.Instance.currentMoney.ToString();
    }
}
