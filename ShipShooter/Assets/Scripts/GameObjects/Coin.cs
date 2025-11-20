using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public GameObject CoinSnd;
    public AudioSource ding;

    
    void Start(){
        
        CoinSnd = GameObject.Find("CoinSnd");
        ding = CoinSnd.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            ding.Play();
            GameObject scoreManagerObj = GameObject.Find("ScoreManager");
            ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
            scoreManager.IncrementMoney();

            Destroy(collision.gameObject);
        }
    }
}
