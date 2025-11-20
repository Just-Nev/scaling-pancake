using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int costIncreaseShootSpeed = 50;
    public int costMagnet = 75;
    public int costIncreaseShootRange = 50;
    public int costShrink = 50;
    public GameObject shieldObj;
    public bool magnetBoughtBool = false;
    public int ShrinkBought = 0;
    public int magnetBought = 0;
    public int ShootSpeed = 0;
    public int shootRange = 0;
    public int score = 0;
    public int HighScore = 0;
    public int money = 0;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI HighscoreText;
    public TextMeshProUGUI CoinTxt;

    private static ScoreManager instance;

    private void Awake()
    {
        // Check if an instance of ScoreManager already exists
        if (instance == null)
        {
            // If not, assign this instance as the singleton instance
            instance = this;

            // Make this object persistent across scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // If an instance already exists, destroy this duplicate
            Destroy(gameObject);
        }


    }


    void Update(){

        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
        HighscoreText = GameObject.Find("HighScoreTxt").GetComponent<TextMeshProUGUI>();
        CoinTxt = GameObject.Find("Coin").GetComponent<TextMeshProUGUI>();

        if (score == 0)
        {
            UpdateScoreText();
        }

    }
      

    public void IncrementScoreBigAsteroid()
    {
        //HighScore += 80;
        score += 80;
        UpdateScoreText();

        if (score > HighScore)
        {
            HighScore = score;
            UpdateScoreText();
        }
    }

    public void IncrementScoreSmallAsteroid()
    {
        //HighScore += 150;
        score += 150;
        UpdateScoreText();

        if (score > HighScore)
        {
            HighScore = score;
            UpdateScoreText();
        }
    }

    public void IncrementMoney(){

        UpdateScoreText();
        money += 20;
        // // Save the updated money value
        // PlayerPrefs.SetInt("Money", money);
        // PlayerPrefs.Save();
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
        HighscoreText.text = HighScore.ToString();
        CoinTxt.text = money.ToString();
    }
}
