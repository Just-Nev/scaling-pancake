using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float cooldownDuration = 0.5f; // Cooldown duration in seconds
    public AudioSource shootSnd;
    private float cooldownTimer = 0f;
    ScoreManager ScoreManager;




    private void Update()
    {


        UpdateCooldown();

        if (Input.GetKeyDown(KeyCode.T) || Input.GetButtonDown("ShockWave") && cooldownTimer <= 0f)
        {
            if(Time.timeScale == 0){
                return;
            }

            FireBullet();
            cooldownTimer = cooldownDuration; // Reset the cooldown timer
        }

    }

    private void UpdateCooldown()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime; // Decrement the cooldown timer by the elapsed time
        }
    }



    private void FireBullet()
    {
        Vector3 spawnPosition = transform.position + transform.up * 0.8f; // Adjust the offset as needed
        Quaternion spawnRotation = transform.rotation;
        GameObject bullet = Instantiate(bulletPrefab, spawnPosition, spawnRotation);
        shootSnd.Play();
    }
}
