using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallAsteroid : MonoBehaviour
{
    float maxX = 9.66f;
    float minX = -9.6f;
    float maxY = 6.37f;
    float minY = -4.45f;

    public GameObject explosion;
    public GameObject SmallParticle;
    public AudioSource exp;

    
    void Start(){
        
        explosion = GameObject.Find("SmallExplosion");
        exp = explosion.GetComponent<AudioSource>();
    }

    void Update(){
        WrapAround();
    }

    private void WrapAround()
    {
        Vector3 currentPosition = transform.position;

        if (currentPosition.x > maxX)
        {
            currentPosition.x = minX;
        }
        else if (currentPosition.x < minX)
        {
            currentPosition.x = maxX;
        }

        if (currentPosition.y > maxY)
        {
            currentPosition.y = minY;
        }
        else if (currentPosition.y < minY)
        {
            currentPosition.y = maxY;
        }

        transform.position = currentPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") )
        {
            Instantiate(SmallParticle, transform.position, Quaternion.identity);

            Destroy(collision.gameObject);
            exp.Play();
            Destroy(gameObject);

            GameObject scoreManagerObj = GameObject.Find("ScoreManager");
            ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
            scoreManager.IncrementScoreSmallAsteroid();
        }

        if (collision.gameObject.CompareTag("ShockWave"))
        {
            Instantiate(SmallParticle, transform.position, Quaternion.identity);

            exp.Play();
            Destroy(gameObject);

            GameObject scoreManagerObj = GameObject.Find("ScoreManager");
            ScoreManager scoreManager = scoreManagerObj.GetComponent<ScoreManager>();
            scoreManager.IncrementScoreSmallAsteroid();
        }

        if (collision.gameObject.CompareTag("Boss1"))
        {
            Instantiate(SmallParticle, transform.position, Quaternion.identity);

            exp.Play();
            Destroy(gameObject);

        }
    }
}
