using UnityEngine;
using System.Collections;

public class RotatingTower : MonoBehaviour
{
    [Header("Orbit & Attack Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    private Transform spot;
    private float fireCooldown = 0f;
    private bool isPlaced = false;
    private float angle = 0f; // current orbit angle in degrees

    public enum TowerType { SingleShot, TripleShot, Burst, LaserBurst }
    [Header("Tower Type Settings")]
    [SerializeField] private TowerType towerType = TowerType.SingleShot;

    void Update()
    {
        if (!isPlaced)
        {
            if (transform.parent != null)
            {
                spot = transform.parent;
                isPlaced = true;

                // Start directly above the spot
                transform.position = spot.position + new Vector3(0f, attackRange, 0f);
                angle = 90f; // corresponds to above the spot
            }
            else
            {
                return; // Not placed yet
            }
        }

        // Orbit logic without changing tower rotation
        angle += rotationSpeed * Time.deltaTime; // increase orbit angle
        if (angle >= 360f) angle -= 360f;

        float rad = angle * Mathf.Deg2Rad;
        float x = spot.position.x + attackRange * Mathf.Cos(rad);
        float y = spot.position.y + attackRange * Mathf.Sin(rad);
        transform.position = new Vector3(x, y, transform.position.z);

        // Shooting logic
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            switch (towerType)
            {
                case TowerType.SingleShot:
                    ShootSingle();
                    Debug.Log("Firing Single Shot");
                    break;

                case TowerType.TripleShot:
                    ShootTriple();
                    Debug.Log("Firing Triple Shot");
                    break;

                case TowerType.Burst:
                    StartCoroutine(ShootBurst(3, 0.1f)); // 3 shots, 0.1 sec apart
                    Debug.Log("Firing Burst");
                    break;

                case TowerType.LaserBurst:
                    StartCoroutine(ShootBurst(5, 0.05f)); // 5 shots, faster
                    Debug.Log("Firing Laser Burst");
                    break;
            }

            fireCooldown = 1f / fireRate;
        }
    }

    // --- Shooting Types ---
    private void ShootSingle()
    {
        FireProjectileAtEnemy();
    }

    private void ShootTriple()
    {
        // Try to fire at one enemy, but with spread
        Collider2D target = FindEnemyInRange();
        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            for (int i = -1; i <= 1; i++)
            {
                float spreadAngle = i * 15f; // -15, 0, +15
                Quaternion rot = Quaternion.Euler(0, 0, spreadAngle) * Quaternion.FromToRotation(Vector3.right, dir);
                GameObject proj = Instantiate(projectilePrefab, transform.position, rot);
                proj.GetComponent<Rigidbody2D>().velocity = rot * Vector3.right * 10f;
            }
        }
    }

    private IEnumerator ShootBurst(int shots, float delay)
    {
        for (int i = 0; i < shots; i++)
        {
            FireProjectileAtEnemy();
            yield return new WaitForSeconds(delay);
        }
    }

    // --- Helper Methods ---
    private void FireProjectileAtEnemy()
    {
        Collider2D target = FindEnemyInRange();
        if (target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            proj.GetComponent<Rigidbody2D>().velocity = dir * 10f;
        }
    }

    private Collider2D FindEnemyInRange()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(spot.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                return hit;
            }
        }
        return null;
    }

    void OnDrawGizmosSelected()
    {
        if (spot != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spot.position, attackRange);
        }
    }
}
