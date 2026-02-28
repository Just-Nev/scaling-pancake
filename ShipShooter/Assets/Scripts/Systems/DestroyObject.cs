using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float Time = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Destroyer", Time);
    }

    void Destroyer()
    {
        Destroy(gameObject);
    }
}
