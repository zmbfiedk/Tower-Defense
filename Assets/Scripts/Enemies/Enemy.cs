using UnityEngine;
using System;

[RequireComponent(typeof(Pathing))]
public class Enemy : MonoBehaviour
{
    // Events for WaveChecker
    public static event Action OnEnemyKilled;
    public static event Action OnEnemyReachedEnd;

    [Header("Stats")]
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int reward = 5;
    [SerializeField] private float speed = 1f;

    private float currentHealth;
    private Pathing pathing;
    private CurrencyManager currencyManager;

    private void Awake()
    {
        currentHealth = maxHealth;
        pathing = GetComponent<Pathing>();
    }

    private void Start()
    {
        // Try to find CurrencyManager in scene
        GameObject obj = GameObject.FindGameObjectWithTag("Currency manager");
        if (obj != null)
            currencyManager = obj.GetComponent<CurrencyManager>();
        else
            Debug.LogWarning("[Enemy] No CurrencyManager found in scene!");
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
        // Add currency if manager is assigned
        if (currencyManager != null)
            currencyManager.AddCurrency(reward);
        else
            Debug.LogWarning("[Enemy] CurrencyManager not assigned!");

        Debug.Log($"[Enemy] {name} died, invoking OnEnemyKilled");
        OnEnemyKilled?.Invoke();

        Destroy(gameObject);
    }

    private void HandleReachedEnd()
    {
        Debug.Log($"[Enemy] {name} reached end, invoking OnEnemyReachedEnd");
        OnEnemyReachedEnd?.Invoke();
        Destroy(gameObject);
    }
}
