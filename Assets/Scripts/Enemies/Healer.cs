using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float healRange = 5f;
    [SerializeField] private float healAmount = 0.1f;
    [SerializeField] private float healInterval = 0.2f; // How often to heal

    private float healCooldown = 0f;

    void Update()
    {
        healCooldown -= Time.deltaTime;
        if (healCooldown <= 0f)
        {
            HealEnemiesInRange();
            healCooldown = healInterval;
        }
    }

    private void HealEnemiesInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, healRange);

        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject) continue; 

            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                Debug.Log("Healed Enemy: " + hit.name);
                var enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(-healAmount); 
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
