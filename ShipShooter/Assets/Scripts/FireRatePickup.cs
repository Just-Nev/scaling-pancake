using UnityEngine;

public class FireRatePickup : MonoBehaviour
{
    [SerializeField] private float newFireRate = 0.1f;

    public void DoubleFireRate()
    {
        RunManager.Instance.currentFireRate = RunManager.Instance.currentFireRate/2;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        RunManager.Instance.currentFireRate = newFireRate;

        Destroy(gameObject);
    }
}
