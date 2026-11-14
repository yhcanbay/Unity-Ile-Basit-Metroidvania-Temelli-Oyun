using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoDisplay : MonoBehaviour
{
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private TextMeshProUGUI currentAmmoTxt;
    [SerializeField] private TextMeshProUGUI storageAmmoTxt;

    void OnEnable()
    {
        Shooting.OnUpdateAllInfo += UpdateAllWeaponInfo;
        Shooting.OnUpdateAmmo +=UpdateAmmoInfo;
    }

    private void UpdateAllWeaponInfo(Sprite weaponSpriteIcon, int currentAmmo, int maxAmmo, int strogeAmmo)
    {
        currentWeaponIcon.sprite = weaponSpriteIcon;
        UpdateAmmoInfo(currentAmmo, maxAmmo, strogeAmmo);
    }

    private void UpdateAmmoInfo(int currentAmmo, int maxAmmo, int strogeAmmo)
    {
        currentAmmoTxt.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
        storageAmmoTxt.text = strogeAmmo.ToString();
    }
}
