using UnityEngine;


public class NextLevelPortal : MonoBehaviour
{
    public GameObject button;
    public SpriteRenderer sp;
    private void OnTriggerEnter2D(Collider2D other)
    {
        var next = other.GetComponent<PlayerNextLevel>();
        if (next != null)
        {
            next.BeginSequenceToCenter();
            sp.enabled = false;
            button.SetActive(true);
        }
    }
}