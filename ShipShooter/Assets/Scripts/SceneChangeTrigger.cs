using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneTrigger : MonoBehaviour
{
    [SerializeField] private bool destroyAfterTrigger = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);

        if (destroyAfterTrigger)
        {
            Destroy(gameObject);
        }
    }
}