using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("Destroyer", 1f);
    }

    void Destroyer()
    {
        Destroy(gameObject);
    }
}
