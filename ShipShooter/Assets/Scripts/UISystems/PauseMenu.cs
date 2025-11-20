using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseMn;
    public GameObject OptionsMn;
    public GameObject AreYouSureMn;
    public GameObject ResumeBtn;

    bool isGamePaused = false;


    public void Update(){

        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Pause"))
        {

            Debug.Log("You have pressed escape");
            if(isGamePaused)
            {
                Resume();
            }
            else
            {
                Paused();
            }
        }
    }

    public void Resume(){
        //What to do when the game resumes
        PauseMn.SetActive(false);
        //OptionsMn.SetActive(false);
        //AreYouSureMn.SetActive(false);
        isGamePaused = false;
        Debug.Log("Game is playing");
        Time.timeScale = 1.0f;
    }

    public void Paused(){

        //What to do when the game is paused
        PauseMn.SetActive(true);
        isGamePaused = true;
        Debug.Log("Game is paused");

        //Clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected game object
        EventSystem.current.SetSelectedGameObject(ResumeBtn);

        Time.timeScale = 0f;

    }

    public void Restart(){
        //Restarts the scene
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        //reset the score
        scoreManager.score = 0;
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu(){    
        //Quits the game
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}
