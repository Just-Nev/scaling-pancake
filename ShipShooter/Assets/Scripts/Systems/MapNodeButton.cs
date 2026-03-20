using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    public string nodeID;

    private Button button;
    private Image image;

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        button.onClick.AddListener(OnClickNode);
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (MapManager.Instance == null)
            return;

        bool unlocked = MapManager.Instance.IsNodeUnlocked(nodeID);
        bool completed = MapManager.Instance.IsNodeCompleted(nodeID);

        button.interactable = unlocked && !completed;

        if (completed)
            image.color = Color.yellow;
        else if (unlocked)
            image.color = Color.white;
        else
            image.color = Color.gray;
    }

    void OnClickNode()
    {
        MapManager.Instance.SelectNode(nodeID);
    }
}
