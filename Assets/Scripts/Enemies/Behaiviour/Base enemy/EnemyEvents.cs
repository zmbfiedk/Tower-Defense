using UnityEngine;
using System;

public class EnemyEvents : MonoBehaviour
{
    public static event Action OnEnemyKilled;
    public static event Action OnEnemyReachedEnd;
    public static event Action OnBossDefeated;

    // NEW: Sends a percentage (0–1) for difficulty adjustment
    public static event Action<float> OnEnemyDeathWithDistance;

    [SerializeField] private bool isBoss;
    private EnemyPath enemyPath; // reference for travel tracking

    private void Awake()
    {
        enemyPath = GetComponent<EnemyPath>();
    }

    public void Die()
    {
        if (isBoss)
            OnBossDefeated?.Invoke();
        else
            OnEnemyKilled?.Invoke();

        // NEW: Report travel progress
        if (enemyPath != null)
            OnEnemyDeathWithDistance?.Invoke(enemyPath.GetTravelProgress());
        else
            OnEnemyDeathWithDistance?.Invoke(0f);

        EnemyReward reward = GetComponent<EnemyReward>();
        reward?.GiveReward();
        Destroy(gameObject);
    }

    public void ReachedEnd()
    {
        if (isBoss)
            OnBossDefeated?.Invoke();
        else
            OnEnemyReachedEnd?.Invoke();

        // NEW: At end, report 100% travel
        OnEnemyDeathWithDistance?.Invoke(1f);

        Destroy(gameObject);
    }
}
