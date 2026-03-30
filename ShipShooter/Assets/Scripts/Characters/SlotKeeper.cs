using UnityEngine;

public class SlotKeeper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ResetPulled()
    {
        GetComponent<Animator>().SetBool("Pulled", false);
    }
}
