using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    ScoreManager ScoreManager;

    void Start(){
        GameObject scoreManager = GameObject.Find("ScoreManager");
        ScoreManager = scoreManager.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScoreManager.ShrinkBought == 1)
        {
            transform.localScale = new Vector2 (1.50f, 1.50f);
        }

        if (ScoreManager.ShrinkBought == 2)
        {
            transform.localScale = new Vector2 (1.45f, 1.45f);
        }

        if (ScoreManager.ShrinkBought == 3)
        {
            transform.localScale = new Vector2 (1.30f, 1.30f);
        }
    }
}
