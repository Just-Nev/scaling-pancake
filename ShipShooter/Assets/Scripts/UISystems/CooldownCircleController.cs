using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownCircleController : MonoBehaviour
{
    public Image fillImage;
    public GameObject player;
    public Vector3 offset = new Vector3(0.5f, 0.5f, 0f); // Adjust the offset as needed

    private void FixedUpdate()
    {
        if (player.activeSelf)
        {
            transform.position = player.transform.position + offset;
            fillImage.enabled = true;
        }
        else
        {
            fillImage.enabled = false;
        }
    }

    public void SetCooldown(float currentTime, float maxTime)
    {
        fillImage.fillAmount = currentTime / maxTime;
    }
}
