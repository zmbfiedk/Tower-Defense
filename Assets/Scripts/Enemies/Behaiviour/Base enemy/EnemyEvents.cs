using System;
using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    public static event Action OnEnemyKilled;
    public static event Action OnEnemyReachedEnd;
    public static event Action OnBossDefeated;

    [SerializeField] private bool isBoss;

    public void Die()
    {
        if (isBoss) OnBossDefeated?.Invoke();
        else OnEnemyKilled?.Invoke();

        Destroy(gameObject);
    }

    public void ReachedEnd()
    {
        if (isBoss) OnBossDefeated?.Invoke();
        else OnEnemyReachedEnd?.Invoke();

        Destroy(gameObject);
    }
}
