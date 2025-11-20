using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceInvaderBoss : MonoBehaviour
{
    public float initialSpeed = 1f;
    public float speedIncrement = 0.1f;
    public float distanceToMoveDown = 1f;
    public float moveDelay = 1f;
    public float moveSpeedCap = 10f;
    public float BossHealth = 10f;
    public float DamageTaken = 0.22f;
    public bool isDead = false;
    public GameObject BossHitParticle;
    public GameObject BossDeathParticle;
    public GameObject BossHealthBarObj;
    public Image BossHealthBar;

    private bool movingRight = true;
    private float moveTimer = 0f;
    private float currentSpeed;
    public float reverseChance = 0.1f; // Adjust this value to control the chance of reversal

    private void Start()
    {
        currentSpeed = initialSpeed;
    }

    private void FixedUpdate()
    {
        moveTimer += Time.fixedDeltaTime;

        if (moveTimer >= moveDelay)
        {
            if (movingRight)
            {
                transform.Translate(Vector3.right * currentSpeed * Time.fixedDeltaTime);

                //if (transform.position.x >= GameManager.Instance.RightBoundary.position.x)
                //{
                //    movingRight = false;
                //    MoveDown();
                //}
            }
            else
            {
                transform.Translate(Vector3.left * currentSpeed * Time.fixedDeltaTime);

                //if (transform.position.x <= GameManager.Instance.LeftBoundary.position.x)
                //{
                //    movingRight = true;
                //    MoveDown();
                //}
            }

            // Randomly reverse the movement direction
            float reverseRoll = Random.value;
            if (reverseRoll <= reverseChance)
            {
                movingRight = !movingRight;
            }

            moveTimer = 0f;
        }
    }

    private void MoveDown()
    {
        Debug.Log(currentSpeed);
        transform.Translate(Vector3.down * distanceToMoveDown);
        if (currentSpeed < moveSpeedCap)
        {
            currentSpeed += speedIncrement;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Instantiate the death particle effect
            Instantiate(BossHitParticle, transform.position + transform.up * -1.25f, Quaternion.identity);
            Destroy(collision.gameObject);
            BossHealth -= DamageTaken;
            BossHealthBar.fillAmount = BossHealth;

            if (BossHealth <= 0)
            {
                isDead = true;
                // Instantiate the death particle effect
                BossHealthBarObj.SetActive(false);
                Instantiate(BossDeathParticle, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
            }
        }
    }
}
