using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float moveSpeed = 5f;
    public float acceleration = 5f;
    ScoreManager ScoreManager;
    public float maxX = 10f;
    public float minX = -10f;
    public float maxY = 10f;
    public float minY = -10f;
    public AudioSource rocketThurst;
    private Rigidbody2D rb;
    private Animator an;
    private bool isPlayingSound = false;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        an = GetComponent<Animator>();
        GameObject scoreManager = GameObject.Find("ScoreManager");
        ScoreManager = scoreManager.GetComponent<ScoreManager>();
    }

    private void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetAxis("Vertical") > 0)
        {
            an.SetBool("isGoing", true);

            // Play the sound only if it's not already playing
            if (!isPlayingSound)
            {
                rocketThurst.Play();
                isPlayingSound = true; // Set the flag to indicate the sound is playing
            }
        }
        else
        {
            // Stop the animation
            an.SetBool("isGoing", false);
            rocketThurst.Stop();
            isPlayingSound = false; // Reset the flag when the key is released
        }


        float rotationInput = Input.GetAxis("Horizontal");
        float accelerationInput = Mathf.Clamp01(Input.GetAxis("Vertical")); // Clamp the input between 0 and 1

        // Rotate the ship
        transform.Rotate(Vector3.forward * -rotationInput * rotationSpeed * Time.deltaTime);

        // Calculate the target velocity based on move speed and acceleration
        Vector2 targetVelocity = transform.up * accelerationInput * acceleration * moveSpeed;

        // Apply the target velocity using AddForce for smoother movement
        rb.AddForce(targetVelocity - rb.velocity);

        // Check if the ship is outside the bounds and wrap around if necessary
        WrapAround();

        //Check if ship should be shrunk
        // if (ScoreManager.ShrinkBought > 0)
        // {
        //     transform.localScale = new Vector2 (1.35f, 1.35f);
        // }

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        // Draw the bounds using Gizmos
        Gizmos.DrawLine(new Vector3(minX, minY, 0f), new Vector3(maxX, minY, 0f));
        Gizmos.DrawLine(new Vector3(maxX, minY, 0f), new Vector3(maxX, maxY, 0f));
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0f), new Vector3(minX, maxY, 0f));
        Gizmos.DrawLine(new Vector3(minX, maxY, 0f), new Vector3(minX, minY, 0f));
    }
}
