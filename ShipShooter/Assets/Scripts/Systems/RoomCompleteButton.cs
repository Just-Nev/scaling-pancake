using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCompleteButton : MonoBehaviour
{
    public void FinishRoom()
    {
        MapManager.Instance.CompleteCurrentNode();
        SceneManager.LoadScene("LevelTransition");
    }
}
