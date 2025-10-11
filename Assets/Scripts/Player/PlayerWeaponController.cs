using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon Sprites")]
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite singleShotSprite;
    [SerializeField] private Sprite tripleShotSprite;
    [SerializeField] private Sprite burstSprite;
    [SerializeField] private Sprite laserBurstSprite;
    [SerializeField] private Sprite rifleSprite;
    [SerializeField] private Sprite freezeThreeSprite;

    [Header("Weapon Display (Optional UI)")]
    [SerializeField] private Image weaponDisplay;

    [Header("Player Weapon SpriteRenderer")]
    [SerializeField] private SpriteRenderer playerWeaponRenderer;

    [Header("Settings")]
    [SerializeField] private float checkInterval = 1f;

    private TowerAttackController.TowerType? currentWeapon = null;
    private List<TowerAttackController.TowerType> unlockedWeapons = new List<TowerAttackController.TowerType>();
    private float checkTimer = 0f;

    private void Start()
    {
        UpdateUnlockedWeapons();
        if (unlockedWeapons.Count > 0)
            currentWeapon = unlockedWeapons[unlockedWeapons.Count - 1];
        UpdateWeaponSprite();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySwitchWeapon(TowerAttackController.TowerType.SingleShot);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySwitchWeapon(TowerAttackController.TowerType.TripleShot);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySwitchWeapon(TowerAttackController.TowerType.Burst);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TrySwitchWeapon(TowerAttackController.TowerType.LaserBurst);
        if (Input.GetKeyDown(KeyCode.Alpha5)) TrySwitchWeapon(TowerAttackController.TowerType.Rifle);
        if (Input.GetKeyDown(KeyCode.Alpha6)) TrySwitchWeapon(TowerAttackController.TowerType.FreezeThree);

        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            UpdateUnlockedWeapons();
            checkTimer = checkInterval;
        }
    }

    private void TrySwitchWeapon(TowerAttackController.TowerType newWeapon)
    {
        if (unlockedWeapons.Contains(newWeapon))
        {
            currentWeapon = newWeapon;
            UpdateWeaponSprite();
            Debug.Log($"✅ Switched to: {currentWeapon}");
        }
        else
        {
            Debug.Log("❌ Weapon locked! Build the correct tower first.");
        }
    }

    private void UpdateUnlockedWeapons()
    {
        TowerAttackController[] towers = FindObjectsOfType<TowerAttackController>();

        unlockedWeapons.Clear();

        foreach (var tower in towers)
        {
            if (!unlockedWeapons.Contains(tower.Type))
            {
                unlockedWeapons.Add(tower.Type);
            }
        }

        // ensure current weapon is valid
        if (currentWeapon.HasValue && !unlockedWeapons.Contains(currentWeapon.Value))
        {
            currentWeapon = unlockedWeapons.Count > 0 ? unlockedWeapons[0] : (TowerAttackController.TowerType?)null;
            UpdateWeaponSprite();
        }
    }

    private void UpdateWeaponSprite()
    {
        Sprite newSprite = defaultSprite;

        if (currentWeapon.HasValue)
        {
            switch (currentWeapon.Value)
            {
                case TowerAttackController.TowerType.SingleShot: newSprite = singleShotSprite; break;
                case TowerAttackController.TowerType.TripleShot: newSprite = tripleShotSprite; break;
                case TowerAttackController.TowerType.Burst: newSprite = burstSprite; break;
                case TowerAttackController.TowerType.LaserBurst: newSprite = laserBurstSprite; break;
                case TowerAttackController.TowerType.Rifle: newSprite = rifleSprite; break;
                case TowerAttackController.TowerType.FreezeThree: newSprite = freezeThreeSprite; break;
            }
        }
        if (weaponDisplay != null) weaponDisplay.sprite = newSprite;
        if (playerWeaponRenderer != null) playerWeaponRenderer.sprite = newSprite;
    }

    public TowerAttackController.TowerType? GetCurrentWeapon() => currentWeapon;
}
