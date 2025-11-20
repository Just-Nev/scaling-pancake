using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public bool dead = false;
    public bool isShieldActive = false;
    public GameObject shield;
    public bool shieldHit = false;
    public Animator GameOverAnim;
    public GameObject deathParticlePrefab; // Particle effect prefab to play upon death
    public GameObject GameoverUI;
    ShipMovement playerMovement; // Reference to the player movement script

    public GameObject explosion;
    public AudioSource exp;

    private void Awake()
    {
        // Get the PlayerMovement component attached to the same object
        playerMovement = GetComponent<ShipMovement>();

        // GameObject scoreManager = GameObject.Find("ScoreManager");
        // ScoreManager = scoreManager.GetComponent<ScoreManager>();

        explosion = GameObject.Find("explosion");
        exp = explosion.GetComponent<AudioSource>();
    }

    public void Update(){
        isShieldActive = shield.activeSelf;
    }

    public void ActivateShield(){
        //isShieldActive = true;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isShieldActive == false && collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("Boss1"))
        {
            CinemachineShake.Instance.ShakeCamera(4f, 1f);
            dead = true;
            Debug.Log("Hit without shield");

            exp.Play();

            // Instantiate the death particle effect
            Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

            // Disable the player movement script
            if (playerMovement != null)
            {
                playerMovement.rocketThurst.Stop();
            }


            // Disable the player object
            gameObject.SetActive(false);

            GameOverAnim.SetBool("IsGameOver", true);

            GameoverUI.SetActive(true);
            // Call a game over function or perform any other actions you want
            Invoke("GameOver",6);
        }

        if(isShieldActive == true && collision.gameObject.CompareTag("Asteroid")){
            //Destroy(collision.gameObject);
            shieldHit = true;
            shield.SetActive(false);
        }

        
    }

    private void GameOver()
    {
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        // Save the player's money using PlayerPrefs
        // PlayerPrefs.SetInt("Money", scoreManager.money);
        // PlayerPrefs.Save();      

        // Add your game over logic here
        Debug.Log("Game Over");

        //reset the score
        scoreManager.score = 0;

        SceneManager.LoadScene(0);
        //Time.timeScale = 0f;
    }
}
