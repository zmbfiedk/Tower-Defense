using UnityEngine;

public class RespawnableToggle : MonoBehaviour
{
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        if (SceneToggleManager.Instance == null)
        {
            var managerGO = new GameObject("SceneToggleManager");
            managerGO.AddComponent<SceneToggleManager>();
        }

        SceneToggleManager.Instance.Register(this);

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 1)
            gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (SceneToggleManager.Instance != null)
            SceneToggleManager.Instance.Unregister(this);
    }

    public void HandleSceneChange(bool enable)
    {
        if (enable)
        {
            transform.position = originalPosition;
            transform.rotation = originalRotation;
            transform.localScale = originalScale;

            // Reactivate the object
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
