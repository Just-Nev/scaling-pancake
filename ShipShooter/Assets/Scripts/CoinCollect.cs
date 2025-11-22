using UnityEngine;

public class CoinCollect : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
        }

    }
}
