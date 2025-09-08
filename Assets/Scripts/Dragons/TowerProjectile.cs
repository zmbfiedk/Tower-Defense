using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;
    [SerializeField] private float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            var enemy = collision.GetComponent<Enemy>();
            if (enemy != null) enemy.TakeDamage(damage);

            Destroy(gameObject);
        }
    }
}
