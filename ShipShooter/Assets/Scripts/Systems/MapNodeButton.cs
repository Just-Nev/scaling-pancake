using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    public string nodeID;

    [Header("State Colors")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = Color.gray;
    public Color completedColor = new Color(0.2f, 1f, 0.2f, 1f);
    public Color blockedColor = new Color(0.4f, 0.1f, 0.1f, 0.85f);

    [Header("Room Type Sprites")]
    public Sprite combatSprite;
    public Sprite shopSprite;
    public Sprite bossSprite;
    public Sprite upgradeRoomSprite;

    [Header("Child Icon Image")]
    public Image iconImage;

    private Button button;
    private Image backgroundImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        backgroundImage = GetComponent<Image>();
    }

    private void Start()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnClickNode);
        }

        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (MapManager.Instance == null || button == null || backgroundImage == null)
            return;

        bool unlocked = MapManager.Instance.IsNodeUnlocked(nodeID);
        bool completed = MapManager.Instance.IsNodeCompleted(nodeID);
        bool blocked = MapManager.Instance.IsNodeBlocked(nodeID);

        button.interactable = unlocked && !completed && !blocked;

        if (completed)
        {
            backgroundImage.color = completedColor;
        }
        else if (blocked)
        {
            backgroundImage.color = blockedColor;
        }
        else if (unlocked)
        {
            backgroundImage.color = unlockedColor;
        }
        else
        {
            backgroundImage.color = lockedColor;
        }

        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (MapManager.Instance == null || iconImage == null)
            return;

        MapNode node = MapManager.Instance.GetNode(nodeID);
        if (node == null)
            return;

        switch (node.roomType)
        {
            case RoomType.Combat:
                iconImage.sprite = combatSprite;
                break;

            case RoomType.Shop:
                iconImage.sprite = shopSprite;
                break;

            case RoomType.Boss:
                iconImage.sprite = bossSprite;
                break;

            case RoomType.Upgrade:
                iconImage.sprite = upgradeRoomSprite;
                break;
        }

        iconImage.color = Color.white;
    }

    public void OnClickNode()
    {
        if (MapManager.Instance != null)
        {
            MapManager.Instance.SelectNode(nodeID);
        }
    }
}
