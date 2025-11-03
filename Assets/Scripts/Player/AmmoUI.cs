using UnityEngine;
using UnityEngine.UI;
using TMPro; // if you’re using TextMeshPro (recommended)

public class PlayerAmmoUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerAttackController playerAttack;
    [SerializeField] private PlayerWeaponController playerWeapon;
    [SerializeField] private TextMeshProUGUI ammoText; // use Text if you prefer

    private TowerAttackController.TowerType? currentWeaponType;

    private void Start()
    {
        if (playerAttack == null)
            playerAttack = FindObjectOfType<PlayerAttackController>();
        if (playerWeapon == null)
            playerWeapon = FindObjectOfType<PlayerWeaponController>();

        if (playerAttack != null)
            playerAttack.OnAmmoChanged += UpdateAmmoDisplay;

        UpdateCurrentWeapon();
    }

    private void Update()
    {   
        if (playerWeapon == null) return;

        TowerAttackController.TowerType? newWeapon = playerWeapon.GetCurrentWeapon();
        if (newWeapon != currentWeaponType)
        {
            currentWeaponType = newWeapon;
            UpdateCurrentWeapon();
        }
    }

    private void UpdateCurrentWeapon()
    {
        if (!currentWeaponType.HasValue || playerAttack == null)
        {
            ammoText.text = "- / -";
            return;
        }

        var type = currentWeaponType.Value;
        int current = playerAttack.GetCurrentAmmo(type);
        int max = playerAttack.GetMaxAmmo(type);
        ammoText.text = $"{current} / {max}";
    }

    private void UpdateAmmoDisplay(TowerAttackController.TowerType type, int current, int max)
    {
        // Only update text if this weapon is the current one
        if (currentWeaponType.HasValue && type == currentWeaponType.Value)
        {
            ammoText.text = $"{current} / {max}";
        }
    }

    private void OnDestroy()
    {
        if (playerAttack != null)
            playerAttack.OnAmmoChanged -= UpdateAmmoDisplay;
    }
}
