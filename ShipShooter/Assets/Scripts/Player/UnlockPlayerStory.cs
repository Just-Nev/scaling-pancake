using UnityEngine;

public class UnlockPlayerStory : MonoBehaviour
{
    public PlayerMovementMobile playerMovement;
    public float TimeToUnlock;
    bool hasBeenUnlocked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasBeenUnlocked == false)
        {
            Invoke("unlockPlayer", TimeToUnlock);
        }

    }

    void unlockPlayer()
    {
        playerMovement.UnlockControls();
        hasBeenUnlocked = true;
    }
}
