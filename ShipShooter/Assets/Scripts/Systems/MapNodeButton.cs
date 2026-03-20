using UnityEngine;
using UnityEngine.UI;

public class MapNodeButton : MonoBehaviour
{
    public string nodeID;
    private Button button;
    private Image image;

    private void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        Refresh();
        button.onClick.AddListener(OnClickNode);
    }

    public void Refresh()
    {
        if (MapManager.Instance == null)
            return;

        bool unlocked = MapManager.Instance.IsNodeUnlocked(nodeID);
        bool completed = MapManager.Instance.IsNodeCompleted(nodeID);

        button.interactable = unlocked;

        if (completed)
        {
            image.color = Color.green;
        }
        else if (unlocked)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.gray;
        }
    }

    void OnClickNode()
    {
        MapManager.Instance.SelectNode(nodeID);
    }
}
