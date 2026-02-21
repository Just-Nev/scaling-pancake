using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableCursorTrigger2D : MonoBehaviour
{
    private void Reset()
    {
        // Make sure the collider is a trigger for hover detection
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnMouseEnter()
    {
        if (CursorManager.Instance != null)
            CursorManager.Instance.RegisterInteractHover(true);
    }

    private void OnMouseExit()
    {
        if (CursorManager.Instance != null)
            CursorManager.Instance.RegisterInteractHover(false);
    }
}
