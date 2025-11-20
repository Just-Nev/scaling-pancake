using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool CanSkip = false;

    public GameObject StartBtn;
    public GameObject SettingsBtn;
    public GameObject SettingsExitBtn;
    public GameObject storeBtn;
    public GameObject storeExitBtn;

    public Animator gameUI;
    public Animator CoinUI;
    PlayerDeath playerDeath;

    // Start is called before the first frame update
    void Start()
    {
        GameObject ship = GameObject.Find("Ship");
        playerDeath = ship.GetComponent<PlayerDeath>();
        
    }

    public void Update(){

        if(playerDeath.dead){
            Invoke("AllowedToSkip", 1.5f);
 
        }

        if(CanSkip){
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit"))
            {
                restartGame();
            }
        }

    }

    void AllowedToSkip(){
        CanSkip = true;
    }

    void restartGame(){
        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        //reset the score
        scoreManager.score = 0;
        SceneManager.LoadScene(0);
    }

    public void Settings()
    {
        //Clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected game object
        EventSystem.current.SetSelectedGameObject(SettingsExitBtn);
    }

    public void SettingsExit() 
    {
        //Clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected game object
        EventSystem.current.SetSelectedGameObject(SettingsBtn);
    }

    public void StoreBtn()
    {
        //Clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected game object
        EventSystem.current.SetSelectedGameObject(storeExitBtn);
    }

    public void StoreExit()
    {
        //Clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected game object
        EventSystem.current.SetSelectedGameObject(storeBtn);
    }

    public void quitGame(){
        Application.Quit();
    }

    public void StartGame()
    {
        //Clear the selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set a new selected game object
        EventSystem.current.SetSelectedGameObject(StartBtn);

        CoinUI.SetBool("IsGameBegun",true);
        gameUI.SetBool("IsGameBegun",true);
        Time.timeScale = 1;
    }

    public void Store(){
        CoinUI.SetBool("IsGameBegun",true);
    }

    public void CLoseStore(){
        CoinUI.SetBool("IsGameBegun",false);
    }
}
