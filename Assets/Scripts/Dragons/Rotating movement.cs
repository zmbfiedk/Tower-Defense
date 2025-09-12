
using UnityEngine;

public class RotatingTower : MonoBehaviour
{


    [Header("Orbit & Attack Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private string  TowerType = "SingleShot";
    private Transform spot;
    private float fireCooldown = 0f;
    private bool isPlaced = false;
    private float angle = 0f; // current orbit angle in degrees

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

        // Calculate position around the spot
        float rad = angle * Mathf.Deg2Rad;
        float x = spot.position.x + attackRange * Mathf.Cos(rad);
        float y = spot.position.y + attackRange * Mathf.Sin(rad);
        transform.position = new Vector3(x, y, transform.position.z);

        // Shooting logic
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f && gameObject.tag == "TripleShot")
        {
            ShootAtEnemy();
            fireCooldown = 1f / fireRate;
            Debug.Log("Firing Triple Shot");
        }
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f )
        {
            ShootAtEnemy();
            fireCooldown = 1f / fireRate;
            Debug.Log("is being fired Singleshot");
        }
    }

    void ShootAtEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(spot.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector3 dir = (hit.transform.position - transform.position).normalized;
                GameObject proj = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                proj.GetComponent<Rigidbody2D>().velocity = dir * 10f;
                break;
            }
        }
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
