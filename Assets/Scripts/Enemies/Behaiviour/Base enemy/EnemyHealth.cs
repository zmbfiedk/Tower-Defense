using UnityEngine;

[RequireComponent(typeof(EnemyEvents))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;

    private EnemyEvents enemyEvents;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyEvents = GetComponent<EnemyEvents>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            enemyEvents.Die();

        }
    }
}
