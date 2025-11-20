using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Transform attachmentPoint; 

    private void Update()
    {
        
        transform.position = attachmentPoint.position;
        transform.rotation = attachmentPoint.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("SmallAst")){
            Debug.Log("Hit with shield");
            gameObject.SetActive(false);
            Destroy(collision.gameObject);
        }
    }

        
}
