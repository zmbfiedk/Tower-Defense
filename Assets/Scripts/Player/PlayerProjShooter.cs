using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileShooter
{
    private Dictionary<TowerAttackController.TowerType, GameObject> projectilePrefabs;
    private float projectileSpeed;

    public ProjectileShooter(Dictionary<TowerAttackController.TowerType, GameObject> prefabs, float speed)
    {
        projectilePrefabs = prefabs;
        float projectileSpeed = speed;
    }

    public void ShootSingle(Vector3 shootPos, Vector3 targetPos, float radius, TowerAttackController.TowerType type)
    {
        Vector3 dir = (targetPos - shootPos).normalized;
        FireProjectile(shootPos, dir, type);
    }

    public void ShootTriple(Vector3 shootPos, Vector3 targetPos, float radius, TowerAttackController.TowerType type)
    {
        Vector3 dir = (targetPos - shootPos).normalized;
        for (int i = -1; i <= 1; i++)
        {
            float spreadAngle = i * 15f;
            Quaternion rot = Quaternion.Euler(0, 0, spreadAngle) * Quaternion.FromToRotation(Vector3.right, dir);
            FireProjectile(shootPos, rot * Vector3.right, type);
        }
    }

    public IEnumerator ShootBurst(Vector3 shootPos, Vector3 targetPos, float radius, int shots, float delay, TowerAttackController.TowerType type)
    {
        for (int i = 0; i < shots; i++)
        {
            ShootSingle(shootPos, targetPos, radius, type);
            yield return new WaitForSeconds(delay);
        }
    }

    private void FireProjectile(Vector3 position, Vector3 dir, TowerAttackController.TowerType type)
    {
        if (!projectilePrefabs.ContainsKey(type)) return;

        GameObject prefab = projectilePrefabs[type];
        GameObject proj = Object.Instantiate(prefab, position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().velocity = dir * projectileSpeed;
    }
}
