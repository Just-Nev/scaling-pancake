using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomCompleteButton : MonoBehaviour
{
    public void FinishRoom()
    {
        Invoke("delayFinish", 1f);
    }

    public void delayFinish()
    {
        MapManager.Instance.CompleteCurrentNode();
        SceneManager.LoadScene("LevelTransition");
    }
}
