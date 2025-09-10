using UnityEngine;
using static UnityEditor.PlayerSettings;

public class EnemyVariable : MonoBehaviour
{
    [SerializeField] private Pathing enemyVariables;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int damage = 1;
    [SerializeField] private int reward = 5;
    [SerializeField] private float maxHealth = 1f;
    [SerializeField] private CurrencyManager currencyManager;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        GameObject obj = GameObject.FindGameObjectWithTag("Currency manager");
        currencyManager = obj.GetComponent<CurrencyManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if collided with a bullet
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject); 
        }
    }

    private void TakeDamage(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        currencyManager.AddCurrency(reward);
        Destroy(gameObject);
    }
}
