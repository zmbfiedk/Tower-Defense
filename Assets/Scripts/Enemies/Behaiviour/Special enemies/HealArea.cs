using UnityEngine;

public class HealArea : MonoBehaviour
{
    [SerializeField] private float healRange = 5f;

    public void HealEnemies(float healAmount)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, healRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
            {
                EnemyHealth health = hit.GetComponent<EnemyHealth>();
                if (health != null)
                    health.TakeDamage(-healAmount);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
