using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadAllScenes : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            ReloadScenes();
        }
    }

    private void ReloadScenes()
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = sceneCount - 1; i >= 0; i--)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            SceneManager.UnloadSceneAsync(loadedScene);
        }

        SceneManager.LoadScene(0);
    }
}
