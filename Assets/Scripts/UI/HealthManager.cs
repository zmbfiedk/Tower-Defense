using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    private PlayerHealth playerHealth;

    [SerializeField] private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        // Find the player object by tag (make sure the player has the tag "Player")
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
        else
        {
            Debug.LogWarning("PlayerHealth not found! Make sure the player is tagged 'Player'.");
        }
    }

    private void Update()
    {
        if (playerHealth != null && textMeshPro != null)
        {
            textMeshPro.text = playerHealth.CurrentHealth.ToString();
        }
    }
}
