using System.Collections;
using UnityEngine;

public class FreezeProjectile : MonoBehaviour
{
    [Tooltip("Fraction of speed to apply while frozen. 0.5 = 50% speed.")]
    public float speedMultiplier = 0.5f;

    [Tooltip("How many seconds the slow lasts on the enemy")]
    public float slowDuration = 2f;

    [Tooltip("Destroy projectile after this many seconds (safety)")]
    public float lifeTime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // 2D trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        TryApplySlow(other.gameObject);
        Destroy(gameObject);
    }

    // 3D trigger fallback
    private void OnTriggerEnter(Collider other)
    {
        TryApplySlow(other.gameObject);
        Destroy(gameObject);
    }

    private void TryApplySlow(GameObject hit)
    {
        var enemy = hit.GetComponent<EnemyPath>();
        if (enemy != null)
        {
            // apply the slow multiplier
            enemy.ApplySpeedMultiplier(speedMultiplier);
            // restore after slowDuration
            StartCoroutine(RestoreAfter(enemy, slowDuration));
        }
    }

    private IEnumerator RestoreAfter(EnemyPath enemy, float delay)
    {
        yield return new WaitForSeconds(delay);
        // restore to normal speed (multiplier 1)
        enemy.ApplySpeedMultiplier(1f);
    }
}
