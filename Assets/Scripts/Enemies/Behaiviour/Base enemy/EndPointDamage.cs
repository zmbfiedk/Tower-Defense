using UnityEngine;

[RequireComponent(typeof(EnemyPath))]
[RequireComponent(typeof(EnemyEvents))]
public class EndPointDamage : MonoBehaviour
{
    [SerializeField] private int damage = 15;

    private EnemyEvents enemyEvents;

    private void Awake()
    {
        enemyEvents = GetComponent<EnemyEvents>();
    }

    private void OnEnable()
    {
        GetComponent<EnemyPath>().OnReachedEnd += DamagePlayer;
    }

    private void OnDisable()
    {
        GetComponent<EnemyPath>().OnReachedEnd -= DamagePlayer;
    }

    private void DamagePlayer()
    {
        // Damage the player
        PlayerHealth.instance.TakeDamage(damage);

        // Kill the enemy
        if (enemyEvents != null)
            enemyEvents.ReachedEnd();  // Use ReachedEnd instead of Die, since enemy reached end
    }
}
