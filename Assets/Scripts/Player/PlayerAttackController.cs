using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerWeaponController))]
public class PlayerAttackController : MonoBehaviour
{
    public event Action<float> OnReload; // non-static

    [Header("Attack Settings")]
    [SerializeField] private float fireRate = 10f;
    [Tooltip("Time in seconds to reload one weapon")]
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private bool autoReloadOnEmpty = true;

    [Header("Magazine - initial (current) ammo)")]
    [SerializeField] private int Pammo = 5;   // SingleShot
    [SerializeField] private int Sammo = 2;   // TripleShot
    [SerializeField] private int Bammo = 7;   // Burst
    [SerializeField] private int Lammo = 9;   // LaserBurst
    [SerializeField] private int Rammo = 30;  // Rifle
    [SerializeField] private int Frammo = 3;  // FreezeThree

    [Header("Magazine Size (max)")]
    [SerializeField] private int PammoMax = 5;
    [SerializeField] private int SammoMax = 3;
    [SerializeField] private int BammoMax = 7;
    [SerializeField] private int LammoMax = 9;
    [SerializeField] private int RammoMax = 60;
    [SerializeField] private int FrammoMax = 3;

    [Header("Projectile Prefabs")]
    [SerializeField] private GameObject normalProjectilePrefab;
    [SerializeField] private GameObject fireProjectilePrefab;
    [SerializeField] private GameObject iceProjectilePrefab;
    [SerializeField] private GameObject laserProjectilePrefab;
    [SerializeField] private GameObject rifleProjectilePrefab;
    [SerializeField] private GameObject freezeProjectilePrefab;

    private float fireCooldown = 0f;
    private PlayerWeaponController weaponController;
    private PlayerProjShooter shooter; // your project already had this type

    private Dictionary<TowerAttackController.TowerType, int> currentAmmo;
    private Dictionary<TowerAttackController.TowerType, int> maxAmmo;
    private Dictionary<TowerAttackController.TowerType, GameObject> prefabMap;

    private bool isReloading = false;

    public event Action<TowerAttackController.TowerType, int, int> OnAmmoChanged;

    private void Awake()
    {
        weaponController = GetComponent<PlayerWeaponController>();

        prefabMap = new Dictionary<TowerAttackController.TowerType, GameObject>
    {
        { TowerAttackController.TowerType.SingleShot, normalProjectilePrefab },
        { TowerAttackController.TowerType.TripleShot, fireProjectilePrefab },
        { TowerAttackController.TowerType.Burst, iceProjectilePrefab },
        { TowerAttackController.TowerType.LaserBurst, laserProjectilePrefab },
        { TowerAttackController.TowerType.Rifle, rifleProjectilePrefab },         // new prefab
        { TowerAttackController.TowerType.FreezeThree, freezeProjectilePrefab }   // new prefab
    };


        currentAmmo = new Dictionary<TowerAttackController.TowerType, int>
        {
            { TowerAttackController.TowerType.SingleShot, Mathf.Clamp(Pammo, 0, PammoMax) },
            { TowerAttackController.TowerType.TripleShot, Mathf.Clamp(Sammo, 0, SammoMax) },
            { TowerAttackController.TowerType.Burst, Mathf.Clamp(Bammo, 0, BammoMax) },
            { TowerAttackController.TowerType.LaserBurst, Mathf.Clamp(Lammo, 0, LammoMax) },
            { TowerAttackController.TowerType.Rifle, Mathf.Clamp(Rammo, 0, RammoMax) },
            { TowerAttackController.TowerType.FreezeThree, Mathf.Clamp(Frammo, 0, FrammoMax) }
        };

        maxAmmo = new Dictionary<TowerAttackController.TowerType, int>
        {
            { TowerAttackController.TowerType.SingleShot, PammoMax },
            { TowerAttackController.TowerType.TripleShot, SammoMax },
            { TowerAttackController.TowerType.Burst, BammoMax },
            { TowerAttackController.TowerType.LaserBurst, LammoMax },
            { TowerAttackController.TowerType.Rifle, RammoMax },
            { TowerAttackController.TowerType.FreezeThree, FrammoMax }
        };

        shooter = new PlayerProjShooter(normalProjectilePrefab, 30f);
        foreach (var kv in currentAmmo)
            OnAmmoChanged?.Invoke(kv.Key, kv.Value, maxAmmo[kv.Key]);
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R))
        {
            var currentWeapon = weaponController.GetCurrentWeapon();
            if (currentWeapon != null)
                TryReload(currentWeapon.Value);
        }

        if (Input.GetButton("Fire1") && fireCooldown <= 0f && !isReloading)
        {
            Fire();
            fireCooldown = 1f / fireRate;
        }
    }

    private void Fire()
    {
        var currentWeapon = weaponController.GetCurrentWeapon();
        if (currentWeapon == null) return;

        var type = currentWeapon.Value;

        if (prefabMap.ContainsKey(type))
            shooter.SetProjectilePrefab(prefabMap[type]);

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        if (!currentAmmo.ContainsKey(type) || currentAmmo[type] <= 0)
        {
            if (autoReloadOnEmpty)
                TryReload(type);
            return;
        }

        switch (type)
        {
            case TowerAttackController.TowerType.SingleShot:
                shooter.ShootSingle(transform.position, mousePos);
                DecreaseAmmo(type, 1);
                break;

            case TowerAttackController.TowerType.TripleShot:
                shooter.ShootTriple(transform.position, mousePos);
                DecreaseAmmo(type, 1);
                break;

            case TowerAttackController.TowerType.Burst:
                StartCoroutine(DoBurstShots(type, 3, 0.1f, mousePos));
                break;

            case TowerAttackController.TowerType.LaserBurst:
                StartCoroutine(DoBurstShots(type, 5, 0.05f, mousePos));
                break;

            case TowerAttackController.TowerType.Rifle:
                // Rifle: single fast shots. Player fireRate controls the speed (set higher in Inspector).
                shooter.ShootSingle(transform.position, mousePos);
                DecreaseAmmo(type, 1);
                break;

            case TowerAttackController.TowerType.FreezeThree:
                // FreezeThree: three freezing projectiles (each projectile applies single-target slow and disappears)
                shooter.ShootTriple(transform.position, mousePos);
                DecreaseAmmo(type, 1);
                break;
        }
    }

    private IEnumerator DoBurstShots(TowerAttackController.TowerType type, int shots, float delay, Vector3 targetPos)
    {
        for (int i = 0; i < shots; i++)
        {
            if (isReloading || !currentAmmo.ContainsKey(type) || currentAmmo[type] <= 0)
                yield break;

            shooter.ShootSingle(transform.position, targetPos);
            DecreaseAmmo(type, 1);
            yield return new WaitForSeconds(delay);
        }
    }

    private void DecreaseAmmo(TowerAttackController.TowerType type, int amount)
    {
        if (!currentAmmo.ContainsKey(type)) return;

        currentAmmo[type] = Mathf.Max(0, currentAmmo[type] - amount);
        OnAmmoChanged?.Invoke(type, currentAmmo[type], maxAmmo[type]);

        if (currentAmmo[type] <= 0 && autoReloadOnEmpty)
            TryReload(type);
    }

    public void TryReload(TowerAttackController.TowerType type)
    {
        if (isReloading) return;
        if (!maxAmmo.ContainsKey(type)) return;
        if (currentAmmo[type] >= maxAmmo[type]) return;

        StartCoroutine(ReloadWeapon(type));
    }

    private IEnumerator ReloadWeapon(TowerAttackController.TowerType type)
    {
        isReloading = true;
        OnReload?.Invoke(reloadTime);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo[type] = maxAmmo[type];
        OnAmmoChanged?.Invoke(type, currentAmmo[type], maxAmmo[type]);

        isReloading = false;
    }

    public int GetCurrentAmmo(TowerAttackController.TowerType type)
    {
        return currentAmmo.ContainsKey(type) ? currentAmmo[type] : 0;
    }

    public int GetMaxAmmo(TowerAttackController.TowerType type)
    {
        return maxAmmo.ContainsKey(type) ? maxAmmo[type] : 0;
    }
}
