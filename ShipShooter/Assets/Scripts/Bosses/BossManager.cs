using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossManager : MonoBehaviour
{
    public float BossSpawnScore = 100f;
    bool setBtn = true;
    bool bossMusicIsPlaying = false;
    bool FirstChoice = false;
    public float transitionDuration = 1f; // Duration of the transition in seconds
    private bool isTransitioning = false;
    public GameObject Boss;
    public GameObject AsteroidSpawner;
    public GameObject BossHealthBar;
    public GameObject wackyPowerUps;
    public GameObject WackyPowerBtn;

    public AudioSource BossMusic;
    public AudioSource GameMusic;

    ScoreManager scoreManager;
    SpaceInvaderBoss spaceInvaderBoss;


    // Start is called before the first frame update
    void Start()
    {
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        scoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        spaceInvaderBoss = Boss.GetComponent<SpaceInvaderBoss>();

    }

    // Update is called once per frame
    void Update()
    {


        if (spaceInvaderBoss.isDead == false)
        {
            if (scoreManager.score > BossSpawnScore)
            {
                SpawnBoss1();
            }
        }

        if (spaceInvaderBoss.isDead == true && FirstChoice == false)
        {
            wackyPowerUps.SetActive(true);

            if (setBtn)
            {
                //Clear the selected object
                EventSystem.current.SetSelectedGameObject(null);
                //Set a new selected game object
                EventSystem.current.SetSelectedGameObject(WackyPowerBtn);

                setBtn = false;
            }



            BossMusic.Stop();
            Time.timeScale = 0;
            
        }
    }

    public void WackyPowerUpsClose(){
        ResetAsteroids();
    }

    void ResetAsteroids() { 

        if (bossMusicIsPlaying)
        {
            FirstChoice = true;
            wackyPowerUps.SetActive(false);
            Time.timeScale = 1f;          
            GameMusic.Play();
            bossMusicIsPlaying = false;
        }
    }

    void SpawnBoss1()
    {
        Boss.SetActive(true);
        BossHealthBar.SetActive(true);
        //AsteroidSpawner.SetActive(false);

        if (!bossMusicIsPlaying)
        {
            BossMusic.Play();
            GameMusic.Stop();
            bossMusicIsPlaying = true;
        }
    }
}
