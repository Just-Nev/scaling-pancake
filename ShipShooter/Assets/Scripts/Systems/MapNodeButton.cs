using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    public string nodeID;

    [Header("Colors (Edit in Inspector)")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;
    public Color completedColor = new Color(0.2f, 1f, 0.2f, 1f);
    public Color blockedColor = new Color(0.4f, 0.1f, 0.1f, 0.85f);

    private Button button;
    private Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (button != null)
            button.onClick.AddListener(OnClickNode);

        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (MapManager.Instance == null || button == null || image == null)
            return;

        bool unlocked = MapManager.Instance.IsNodeUnlocked(nodeID);
        bool completed = MapManager.Instance.IsNodeCompleted(nodeID);
        bool blocked = MapManager.Instance.IsNodeBlocked(nodeID);

        // Only clickable if available
        button.interactable = unlocked && !completed && !blocked;

        //Color logic
        if (completed)
        {
            image.color = completedColor;
        }
        else if (blocked)
        {
            image.color = blockedColor;
        }
        else if (unlocked)
        {
            image.color = unlockedColor;
        }
        else
        {
            image.color = lockedColor;
        }
    }

    private void OnClickNode()
    {
        if (MapManager.Instance != null)
        {
            MapManager.Instance.SelectNode(nodeID);
        }
    }
}
