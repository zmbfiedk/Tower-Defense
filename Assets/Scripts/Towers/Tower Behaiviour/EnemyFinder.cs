using UnityEngine;

public static class EnemyFinder
{
    public static Collider2D Find(Vector3 center, float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(center, radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy") || hit.CompareTag("Boss"))
                return hit;
        }
        return null;
    }
}
