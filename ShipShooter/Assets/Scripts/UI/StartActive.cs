using UnityEngine;

public class StartActive : MonoBehaviour
{
    [Header("Objects to Activate")]
    [SerializeField] private GameObject[] objectsToActivate;

    [Header("Activate Automatically")]
    [SerializeField] private bool activateOnStart = true;

    void Start()
    {
        if (activateOnStart)
        {
            ActivateAll();
        }
    }

    // Public function so you can call it from other scripts, UI buttons, story events, etc.
    public void ActivateAll()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    // Optional: function to deactivate all
    public void DeactivateAll()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
