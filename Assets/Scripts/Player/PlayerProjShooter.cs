using UnityEngine;
using System.Collections;

public class PlayerProjShooter
{
    private GameObject projectilePrefab;
    private float projectileSpeed;

    public PlayerProjShooter(GameObject prefab, float speed)
    {
        projectilePrefab = prefab;
        projectileSpeed = speed;
    }

    public void SetProjectilePrefab(GameObject prefab)
    {
        projectilePrefab = prefab;
    }

    public void ShootSingle(Vector3 shootPos, Vector3 targetPos)
    {
        Vector3 dir = (targetPos - shootPos).normalized;
        FireProjectile(shootPos, dir);
    }

    public void ShootTriple(Vector3 shootPos, Vector3 targetPos)
    {
        Vector3 dir = (targetPos - shootPos).normalized;
        for (int i = -1; i <= 1; i++)
        {
            float spreadAngle = i * 15f;
            Quaternion rot = Quaternion.Euler(0, 0, spreadAngle);
            Vector3 spreadDir = rot * dir;
            FireProjectile(shootPos, spreadDir);
        }
    }

    public IEnumerator ShootBurst(Vector3 shootPos, Vector3 targetPos, int shots, float delay)
    {
        for (int i = 0; i < shots; i++)
        {
            ShootSingle(shootPos, targetPos);
            yield return new WaitForSeconds(delay);
        }
    }

    private void FireProjectile(Vector3 position, Vector3 dir)
    {
        GameObject proj = Object.Instantiate(projectilePrefab, position, Quaternion.identity);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

        if (rb != null)
            rb.velocity = dir * projectileSpeed;
        else
            Debug.LogError("Projectile prefab is missing Rigidbody2D!");
    }
}
