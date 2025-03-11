using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Views
{
    public class AmmoView : MonoBehaviour
    {
        [SerializeField] private Image weaponIcon;
        [SerializeField] private TMP_Text ammoCurrentText;
        [SerializeField] private TMP_Text ammoInventoryText;

        public void UpdateAmmo(int currentAmmo, int inventoryAmmo)
        {
            ammoCurrentText.SetText(currentAmmo.ToString());
            ammoInventoryText.SetText($"/ {inventoryAmmo}");
        }

        public void UpdateAmmoIcon(Sprite spriteWeapon)
        {
            weaponIcon.sprite = spriteWeapon;
        }
    }
}
