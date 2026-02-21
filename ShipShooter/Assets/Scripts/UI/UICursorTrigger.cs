using UnityEngine;
using UnityEngine.EventSystems;

public class UICursorTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public bool showInteractOnHover = true;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!showInteractOnHover) return;

        if (CursorManager.Instance != null)
            CursorManager.Instance.RegisterInteractHover(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!showInteractOnHover) return;

        if (CursorManager.Instance != null)
            CursorManager.Instance.RegisterInteractHover(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // CursorManager already switches to Clicking on GetMouseButton(0),
        // but this makes it feel instant if you ever change input logic later.
        // Optional: no-op.
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Optional: no-op.
    }
}
