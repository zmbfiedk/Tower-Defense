using UnityEngine;

[RequireComponent(typeof(EnemyEvents))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private GameObject healthBarPrefab; // assign prefab in inspector

    private float currentHealth;
    private float baseMaxHealth;
    private EnemyEvents enemyEvents;

    private EnemyHealthBar healthBar;

    private void Awake()
    {
        baseMaxHealth = maxHealth;
        currentHealth = maxHealth;
        enemyEvents = GetComponent<EnemyEvents>();
    }

    private void Start()
    {
        if (healthBarPrefab != null)
        {
            // Instantiate the health bar and make it follow this enemy
            GameObject barObj = Instantiate(healthBarPrefab);
            healthBar = barObj.GetComponent<EnemyHealthBar>();
            healthBar.Initialize(transform);
            healthBar.UpdateHealth(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            enemyEvents.Die();

            if (healthBar != null)
                Destroy(healthBar.gameObject);
        }
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        if (multiplier <= 0f) multiplier = 1f;
        float oldMax = maxHealth;
        maxHealth = baseMaxHealth * multiplier;

        if (oldMax > 0f)
        {
            float percent = currentHealth / oldMax;
            currentHealth = Mathf.Clamp(percent * maxHealth, 0f, maxHealth);
        }
        else
        {
            currentHealth = maxHealth;
        }

        if (healthBar != null)
            healthBar.UpdateHealth(currentHealth, maxHealth);
    }

    public float GetBaseMaxHealth() => baseMaxHealth;
}
