using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggleManager : MonoBehaviour
{
    public static SceneToggleManager Instance { get; private set; }

    private readonly List<RespawnableToggle> registered = new List<RespawnableToggle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Register(RespawnableToggle toggle)
    {
        if (toggle != null && !registered.Contains(toggle))
            registered.Add(toggle);
    }

    public void Unregister(RespawnableToggle toggle)
    {
        if (toggle != null)
            registered.Remove(toggle);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isScene1 = scene.buildIndex == 1;

        foreach (var toggle in registered.ToArray())
        {
            if (toggle == null)
            {
                registered.Remove(toggle);
                continue;
            }

            toggle.HandleSceneChange(isScene1);
        }
    }
}
