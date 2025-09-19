using UnityEngine;
using System;

[RequireComponent(typeof(Pathing))]
public class Enemy : MonoBehaviour
{
    // Enemy events
    public static event Action OnEnemyKilled;
    public static event Action OnEnemyReachedEnd;

    //Boss events
    public static event Action OnBossDefeated;

    [Header("Stats")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int reward = 5;
    [SerializeField] private float speed;

    private float currentHealth;
    private Pathing pathing;
    private CurrencyManager currencyManager;

    private bool isBoss;

    private void Awake()
    {
        currentHealth = maxHealth;
        pathing = GetComponent<Pathing>();

        isBoss = CompareTag("Boss");
    }

    private void Start()
    {
        // Currency manager
        GameObject obj = GameObject.FindGameObjectWithTag("Currency manager");
        if (obj != null)
            currencyManager = obj.GetComponent<CurrencyManager>();
        else
            Debug.LogWarning("[Enemy] No CurrencyManager found in scene!");
        speed = pathing.Speed;
    }

    private void OnEnable()
    {
        if (pathing != null)
            pathing.OnReachedEnd += HandleReachedEnd;
    }

    private void OnDisable()
    {
        if (pathing != null)
            pathing.OnReachedEnd -= HandleReachedEnd;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1f);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log($"[Enemy] {name} took {amount} damage, current health: {currentHealth}");

        if (currentHealth <= 0f)
            Die();
    }

    private void Die()
    {
        if (currencyManager != null)
            currencyManager.AddCurrency(reward);
        else
            Debug.LogWarning("[Enemy] CurrencyManager not assigned!");

        Debug.Log($"[Enemy] {name} died.");

        if (isBoss)
        {
            Debug.Log("[Enemy] Boss defeated, invoking OnBossDefeated!");
            OnBossDefeated?.Invoke();
        }
        else
        {
            Debug.Log("[Enemy] Normal enemy killed, invoking OnEnemyKilled!");
            OnEnemyKilled?.Invoke();
        }

        Destroy(gameObject);
    }

    private void HandleReachedEnd()
    {
        Debug.Log($"[Enemy] {name} reached end.");

        if (isBoss)
        {
            Debug.Log("[Enemy] Boss reached end, treating as defeat!");
            OnBossDefeated?.Invoke(); 
        }
        else
        {
            OnEnemyReachedEnd?.Invoke();
        }

        Destroy(gameObject);
    }
}
