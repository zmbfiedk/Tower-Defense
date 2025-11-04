using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ReloadAllScenes : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartCoroutine(ReloadAll());
        }
    }

    private IEnumerator ReloadAll()
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = sceneCount - 1; i >= 0; i--)
        {
            Scene loadedScene = SceneManager.GetSceneAt(i);
            yield return SceneManager.UnloadSceneAsync(loadedScene);
        }

        yield return SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);

        yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

        Debug.Log("Scenes 0 and 1 reloaded successfully!");
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(0));
    }
}
