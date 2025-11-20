using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    float maxX = 9.66f;
    float minX = -9.6f;
    float maxY = 6.37f;
    float minY = -4.45f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Wrap();
    }

        private void Wrap()
        {
            Vector3 currentPosition = transform.position;

            if (currentPosition.x > maxX)
            {
                currentPosition.x = minX;
            }
            else if (currentPosition.x < minX)
            {
                currentPosition.x = maxX;
            }

            if (currentPosition.y > maxY)
            {
                currentPosition.y = minY;
            }
            else if (currentPosition.y < minY)
            {
                currentPosition.y = maxY;
            }

            transform.position = currentPosition;
        }
}
