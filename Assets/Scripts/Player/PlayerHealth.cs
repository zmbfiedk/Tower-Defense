using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    [SerializeField] private int maxHealth = 100;
    [SerializeField]private int currentHealth;

    public int currenthealth => currentHealth;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject); 

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("Player took " + amount + " damage. Current HP: " + currentHealth);
    }

    private void Die()
    {
        Debug.Log("Player died!");

    }
}
