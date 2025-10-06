using UnityEngine;
using System.Collections;

public class PlayerProjShooter
{
    public class ProjectileShooter
    {
        private GameObject projectilePrefab;
        private float projectileSpeed;

        public ProjectileShooter(GameObject prefab, float speed)
        {
            projectilePrefab = prefab;
            projectileSpeed = speed;
        }

        private Collider2D FindEnemy(Vector3 spotPosition, float radius, bool canSeeInvisible)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(spotPosition, radius);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
                {
                    InvisibleEnemy invisible = hit.GetComponent<InvisibleEnemy>();
                    if (invisible != null && invisible.IsInvisible && !canSeeInvisible)
                        continue;

                    return hit;
                }
            }
            return null;
        }

        public void ShootSingle(Vector3 shootPos, Vector3 spotPosition, float radius, bool canSeeInvisible = false)
        {
            var target = FindEnemy(spotPosition, radius, canSeeInvisible);
            if (target != null)
            {
                Vector3 dir = (target.transform.position - shootPos).normalized;
                FireProjectile(shootPos, dir);
            }
        }

        public void ShootTriple(Vector3 shootPos, Vector3 spotPosition, float radius, bool canSeeInvisible = false)
        {
            var target = FindEnemy(spotPosition, radius, canSeeInvisible);
            if (target != null)
            {
                Vector3 dir = (target.transform.position - shootPos).normalized;
                for (int i = -1; i <= 1; i++)
                {
                    float spreadAngle = i * 15f;
                    Quaternion rot = Quaternion.Euler(0, 0, spreadAngle);
                    Vector3 spreadDir = rot * dir;
                    FireProjectile(shootPos, spreadDir);
                }
            }
        }

        public IEnumerator ShootBurst(Vector3 shootPos, Vector3 spotPosition, float radius, int shots, float delay, bool canSeeInvisible = false)
        {
            for (int i = 0; i < shots; i++)
            {
                ShootSingle(shootPos, spotPosition, radius, canSeeInvisible);
                yield return new WaitForSeconds(delay);
            }
        }

        private void FireProjectile(Vector3 position, Vector3 dir)
        {
            var proj = Object.Instantiate(projectilePrefab, position, Quaternion.identity);
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = dir * projectileSpeed;
            }
            else
            {
                Debug.LogError("Projectile prefab is missing Rigidbody2D component!");
            }
        }
    }
}