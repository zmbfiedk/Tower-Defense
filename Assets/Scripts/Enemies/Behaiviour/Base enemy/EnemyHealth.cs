using UnityEngine;

[RequireComponent(typeof(EnemyEvents))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    private float currentHealth;

    private EnemyEvents enemyEvents;

    // store base so multipliers are relative to original prefab value
    private float baseMaxHealth;

    private void Awake()
    {
        baseMaxHealth = maxHealth;
        currentHealth = maxHealth;
        enemyEvents = GetComponent<EnemyEvents>();
    }

    private void OnEnable()
    {
        // If there are legacy listeners of WaveChecker.On10WavesCompleted, they must be removed/updated.
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            enemyEvents.Die();
        }
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        if (multiplier <= 0f) multiplier = 1f;
        float oldMax = maxHealth;
        maxHealth = baseMaxHealth * multiplier;

        // keep same percentage of health after scaling
        if (oldMax > 0f)
        {
            float percent = currentHealth / oldMax;
            currentHealth = Mathf.Clamp(percent * maxHealth, 0f, maxHealth);
        }
        else
        {
            currentHealth = maxHealth;
        }

    }

    // public getter for EnemyScaler
    public float GetBaseMaxHealth() => baseMaxHealth;
}
