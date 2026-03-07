using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroVideoSkip : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string nextSceneName = "MainMenu";

    void Start()
    {
        videoPlayer.loopPointReached += EndReached;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left click
        {
            SkipVideo();
        }
    }

    void SkipVideo()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    void EndReached(VideoPlayer vp)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
