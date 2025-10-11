using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RotatingMovement))]
public class TowerAttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private GameObject projectilePrefab;

    public enum TowerType { SingleShot, TripleShot, Burst, LaserBurst, Rifle, FreezeThree }
    [SerializeField] private TowerType towerType = TowerType.SingleShot;

    // Expose the type without naming conflict
    public TowerType Type => towerType;

    private float fireCooldown = 0f;
    private RotatingMovement orbit;
    private ProjectileShooter shooter;

    private void Awake()
    {
        orbit = GetComponent<RotatingMovement>();
        shooter = new ProjectileShooter(projectilePrefab, 30f);
    }

    private void Update()
    {
        if (!orbit.IsPlaced) return;

        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    private void Fire()
    {
        bool canSeeInvisible = (towerType == TowerType.LaserBurst || towerType == TowerType.Rifle);

        switch (towerType)
        {
            case TowerType.SingleShot:
                shooter.ShootSingle(transform.position, orbit.GetSpotPosition(), orbit.GetRadius(), canSeeInvisible);
                break;

            case TowerType.TripleShot:
                shooter.ShootTriple(transform.position, orbit.GetSpotPosition(), orbit.GetRadius(), canSeeInvisible);
                break;

            case TowerType.Burst:
                StartCoroutine(shooter.ShootBurst(transform.position, orbit.GetSpotPosition(), orbit.GetRadius(), 3, 0.1f, canSeeInvisible));
                break;

            case TowerType.LaserBurst:
                StartCoroutine(shooter.ShootBurst(transform.position, orbit.GetSpotPosition(), orbit.GetRadius(), 5, 0.05f, canSeeInvisible));
                break;

            case TowerType.Rifle:
                // Rifle: fast, accurate single shots. Tweak this tower's fireRate in Inspector.
                shooter.ShootSingle(transform.position, orbit.GetSpotPosition(), orbit.GetRadius(), canSeeInvisible);
                break;

            case TowerType.FreezeThree:
                // FreezeThree: shoots triple freeze projectiles (each projectile slows first enemy hit).
                shooter.ShootTriple(transform.position, orbit.GetSpotPosition(), orbit.GetRadius(), canSeeInvisible);
                break;
        }
    }
}
