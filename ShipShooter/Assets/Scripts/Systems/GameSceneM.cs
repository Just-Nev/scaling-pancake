using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneM : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Loads test map scene
    public void LevelTrans1()
    {
        Invoke("LevelTransDelay", 3);
    }

    void LevelTransDelay() 
    {
        SceneManager.LoadScene(3);

    }

    //Loads test power up scene
    public void LevelPower()
    {
        Invoke("LevelPowerDelay", 3);
    }

    void LevelPowerDelay()
    {
        SceneManager.LoadScene(4);
    }

    public void testSceneLoader()
    {
        SceneManager.LoadScene(2);
    }
}
