using UnityEngine;
using System;

public class BossEnemy : MonoBehaviour
{
    public static event Action OnBossDefeated;

    [SerializeField] private float health = 100f;

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("[BossEnemy] Boss Defeated!");
        OnBossDefeated?.Invoke();
        Destroy(gameObject);
    }
}
