using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    public float magnetRange = 5f;
    public float magnetForce = 10f;
    ScoreManager ScoreManager;

    void Start(){
        GameObject scoreManager = GameObject.Find("ScoreManager");
        ScoreManager = scoreManager.GetComponent<ScoreManager>();
    }

    void Update()
    {

        if(ScoreManager.magnetBought == 2){
            magnetRange = 2f;
        }

        if(ScoreManager.magnetBought == 3){
            magnetRange = 3.5f;
        }

        if(ScoreManager.magnetBought > 0){

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, magnetRange);

            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("coin"))
                {

                    Vector2 direction = (transform.position - collider.transform.position).normalized;
                    Rigidbody2D coinRigidbody = collider.GetComponent<Rigidbody2D>();
                    coinRigidbody.AddForce(direction * magnetForce, ForceMode2D.Force);
                }
            }
        }


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}
