using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    [Header("Pause Settings")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private bool isPaused = false;
    private GameObject[] currencyManagers;
    private GameObject pauseMenuUI;

    void Start()
    {
        // Find the pause menu by tag
        pauseMenuUI = GameObject.FindWithTag("PauseMenu");
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false); // Ensure it's hidden at start
        else
            Debug.LogWarning("No GameObject with tag 'PauseMenu' found!");
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Disable all objects named "CurrencyManager"
        currencyManagers = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in currencyManagers)
        {
            if (obj.name == "CurrencyManager")
                obj.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;


        // Re-enable all objects named "CurrencyManager"
        if (currencyManagers != null)
        {
            foreach (GameObject obj in currencyManagers)
            {
                if (obj != null && obj.name == "CurrencyManager")
                    obj.SetActive(true);
            }
        }
    }

    // Optional: UI button method to quit game
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
