using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 1f;

    [Header("Projectile Prefabs")]
    [SerializeField] private GameObject normalProjectilePrefab;
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private GameObject iceProjectilePrefab;
    [SerializeField] private GameObject laserProjectilePrefab;

    private float fireCooldown = 0f;
    private PlayerWeaponController weaponController;
    private ProjectileShooter shooter;

    private void Awake()
    {
        weaponController = GetComponent<PlayerWeaponController>();

        // Map weapon types to projectile prefabs
        var prefabs = new System.Collections.Generic.Dictionary<TowerAttackController.TowerType, GameObject>
        {
            { TowerAttackController.TowerType.SingleShot, normalProjectilePrefab },
            { TowerAttackController.TowerType.TripleShot, fireProjectilePrefab },
            { TowerAttackController.TowerType.Burst, iceProjectilePrefab },
            { TowerAttackController.TowerType.LaserBurst, laserProjectilePrefab }
        };

        shooter = new ProjectileShooter(prefabs, 30f);
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        // Fire when left mouse button pressed and cooldown is ready
        if (Input.GetButton("Fire1") && fireCooldown <= 0f)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    private void Fire()
    {
        var currentWeapon = weaponController.GetCurrentWeapon();
        if (currentWeapon == null) return; // No weapon equipped

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        switch (currentWeapon.Value)
        {
            case TowerAttackController.TowerType.SingleShot:
                shooter.ShootSingle(transform.position, mousePos, 0f, currentWeapon.Value);
                break;

            case TowerAttackController.TowerType.TripleShot:
                shooter.ShootTriple(transform.position, mousePos, 0f, currentWeapon.Value);
                break;

            case TowerAttackController.TowerType.Burst:
                StartCoroutine(shooter.ShootBurst(transform.position, mousePos, 0f, 3, 0.1f, currentWeapon.Value));
                break;

            case TowerAttackController.TowerType.LaserBurst:
                StartCoroutine(shooter.ShootBurst(transform.position, mousePos, 0f, 5, 0.05f, currentWeapon.Value));
                break;
        }
    }
}
