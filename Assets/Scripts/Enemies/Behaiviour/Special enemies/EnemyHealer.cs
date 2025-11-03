using UnityEngine;

public class EnemyHealer : MonoBehaviour
{
    [SerializeField] private float healAmount = 0.1f;
    [SerializeField] private float healInterval = 0.2f;
    private float healCooldown = 0f;

    [SerializeField] private HealArea healArea;

    private void Update()
    {
        healCooldown -= Time.deltaTime;
        if (healCooldown <= 0f)
        {
            healArea.HealEnemies(healAmount);
            healCooldown = healInterval;
        }
    }
}
