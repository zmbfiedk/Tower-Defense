using UnityEngine;
using TMPro;

public class HealthManager : MonoBehaviour
{
    private PlayerHealth playerHealth;

    [SerializeField] private TextMeshProUGUI textMeshPro; 

    private void Start()
    {
        // Find player by tag
        GameObject healthObj = GameObject.FindWithTag("MainCamera");
        if (healthObj != null)
        {
            playerHealth = healthObj.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (playerHealth != null && textMeshPro != null)
        {
            textMeshPro.text = playerHealth.currenthealth.ToString();
        }
    }
}
