using UnityEngine;
using TMPro;

public class CoinCollect : MonoBehaviour
{
    [Header("Score Settings")]
    public int score = 0;
    public TextMeshProUGUI scoreText;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            Destroy(col.gameObject);

            // Add score
            score++;

            // Update TextMeshPro
            if (scoreText != null)
                scoreText.text = score.ToString();
        }
    }
}


