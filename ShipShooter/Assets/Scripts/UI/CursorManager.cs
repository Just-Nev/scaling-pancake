using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    public enum CursorState { Idle, CanInteract, Clicking }

    [Header("Cursor Textures")]
    public Texture2D idleCursor;
    public Texture2D interactCursor;
    public Texture2D clickCursor;

    [Header("Hotspot (pixel position inside texture)")]
    public Vector2 idleHotspot = Vector2.zero;
    public Vector2 interactHotspot = Vector2.zero;
    public Vector2 clickHotspot = Vector2.zero;

    [Header("Optional")]
    public CursorMode cursorMode = CursorMode.Auto;

    private int _hoverInteractCount = 0; // supports overlapping colliders
    private bool _isClicking = false;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        ApplyCursor(GetDesiredState());
    }

    private void Update()
    {
        // Clicking state when holding button
        bool nowClicking = Input.GetMouseButton(0);
        if (nowClicking != _isClicking)
        {
            _isClicking = nowClicking;
            ApplyCursor(GetDesiredState());
        }
    }

    // Called by hover triggers
    public void RegisterInteractHover(bool hovering)
    {
        _hoverInteractCount += hovering ? 1 : -1;
        if (_hoverInteractCount < 0) _hoverInteractCount = 0;

        ApplyCursor(GetDesiredState());
    }

    private CursorState GetDesiredState()
    {
        if (_isClicking) return CursorState.Clicking;
        if (_hoverInteractCount > 0) return CursorState.CanInteract;
        return CursorState.Idle;
    }

    private void ApplyCursor(CursorState state)
    {
        switch (state)
        {
            case CursorState.Clicking:
                if (clickCursor != null)
                    Cursor.SetCursor(clickCursor, clickHotspot, cursorMode);
                break;

            case CursorState.CanInteract:
                if (interactCursor != null)
                    Cursor.SetCursor(interactCursor, interactHotspot, cursorMode);
                break;

            default:
                if (idleCursor != null)
                    Cursor.SetCursor(idleCursor, idleHotspot, cursorMode);
                break;
        }
    }
}
