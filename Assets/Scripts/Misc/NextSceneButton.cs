using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneButton : MonoBehaviour
{
    [SerializeField] private Button nextButton; 

    private void Start()
    {
        nextButton.onClick.AddListener(LoadNextScene);
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
