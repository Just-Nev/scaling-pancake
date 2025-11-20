using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StoreManager : MonoBehaviour
{
    public PlayerDeath PlayerDeath;
    public ScoreManager ScoreManager;
    TextMeshProUGUI MoneyTxt;

    [Header("ShootSpeed Button")]
    public TextMeshProUGUI ShootSpeedCostTxt;
    



    [Header("Magnet Button")]
    public TextMeshProUGUI MagnetCostTxt;



    [Header("ShootRange Button")]
    public TextMeshProUGUI ShootRangeTxt;
    


    [Header("Shrink Button")]
    public TextMeshProUGUI ShrinkTxt;
    
 
    [Header("Audio")]
    public AudioSource BoughtSnd;
    public AudioSource ErrorSnd;
    

    void Start(){

        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        ScoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        GameObject ShipObj = GameObject.Find("Ship");
        PlayerDeath = ShipObj.GetComponent<PlayerDeath>();   
        UpdateScoreText();
        
    }


    void Update(){

        GameObject scoreManagerObj = GameObject.Find("ScoreManager");
        ScoreManager = scoreManagerObj.GetComponent<ScoreManager>();

        MoneyTxt = GameObject.Find("Coin").GetComponent<TextMeshProUGUI>();
        MoneyTxt.text = ScoreManager.money.ToString();     

    }

    public void IncreaseShootSpeed(){

        
        if(ScoreManager.money >= ScoreManager.costIncreaseShootSpeed && ScoreManager.ShootSpeed < 3){

            Debug.Log("You have bought increase shot speed");           
            ScoreManager.money -= ScoreManager.costIncreaseShootSpeed;
            ScoreManager.costIncreaseShootSpeed += 50;
            ScoreManager.ShootSpeed += 1;
            BoughtSnd.Play();
            UpdateScoreText();
            //Debug.Log(costIncreaseShootSpeed);
            Debug.Log(ScoreManager.ShootSpeed);

        }else
        {

            Debug.Log("Cant afford");
            ErrorSnd.Play();

        }

        if(ScoreManager.ShootSpeed == 3){
            ShootSpeedCostTxt.text = "MAX";
        }

    }

    public void Magnet(){

        
        if(ScoreManager.money >= ScoreManager.costMagnet && ScoreManager.magnetBought < 3){

            Debug.Log("You have bought magnet");
            ScoreManager.money -= ScoreManager.costMagnet;
            ScoreManager.costMagnet += 75;
            ScoreManager.magnetBought += 1;
            BoughtSnd.Play();
            UpdateScoreText();
            //Debug.Log(costMagnet);
            Debug.Log(ScoreManager.magnetBought);

        }else{

            Debug.Log("Cant afford");
            ErrorSnd.Play();
        }

        if(ScoreManager.magnetBought == 3){
            MagnetCostTxt.text = "MAX";
        }
    }

    public void shootRange(){

        
        if(ScoreManager.money >= ScoreManager.costIncreaseShootRange && ScoreManager.shootRange < 3){

            Debug.Log("You have bought a shrink");
            ScoreManager.money -= ScoreManager.costIncreaseShootRange;   
            ScoreManager.costIncreaseShootRange += 50;
            ScoreManager.shootRange += 1;         
            BoughtSnd.Play();
            UpdateScoreText();
            //Debug.Log(costMagnet);
            Debug.Log(ScoreManager.magnetBought);

        }else{

            Debug.Log("Cant afford");
            ErrorSnd.Play();
        }

        if(ScoreManager.shootRange == 3){
            ShootRangeTxt.text = "MAX";
        }
    }



    public void shrinkShip(){

        
        if(ScoreManager.money >= ScoreManager.costShrink && ScoreManager.ShrinkBought < 3){

            Debug.Log("You have bought shockwave");
            ScoreManager.money -= ScoreManager.costShrink;
            ScoreManager.costShrink += 50;
            ScoreManager.ShrinkBought += 1;
            BoughtSnd.Play();
            UpdateScoreText();
            //Debug.Log(costShrink);
            Debug.Log(ScoreManager.ShrinkBought);

        }else{
            
            Debug.Log("Cant afford");
            ErrorSnd.Play();
        }

        
        if(ScoreManager.ShrinkBought == 3){
            ShrinkTxt.text = "MAX";
        }
    }

    private void UpdateScoreText()
    {

        if(ScoreManager.ShootSpeed != 3){
            ShootSpeedCostTxt.text = ScoreManager.costIncreaseShootSpeed.ToString();
        }else{
            ShootSpeedCostTxt.text = "MAX";
        }  

        if(ScoreManager.magnetBought != 3){
            MagnetCostTxt.text = ScoreManager.costMagnet.ToString();
        }else{
            MagnetCostTxt.text = "MAX";
        }

        if(ScoreManager.shootRange != 3){
            ShootRangeTxt.text = ScoreManager.costIncreaseShootRange.ToString();
        
        }else{
            ShootRangeTxt.text = "MAX";
        }

        if(ScoreManager.ShrinkBought != 3){
            ShrinkTxt.text = ScoreManager.costShrink.ToString();
        }else{
            ShrinkTxt.text = "MAX";
        }
        
    }
}
