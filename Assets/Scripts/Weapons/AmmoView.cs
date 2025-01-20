using UnityEngine;
using TMPro;

public class AmmoView : MonoBehaviour
{
    [SerializeField] private TMP_Text ammoCurrentText;
    [SerializeField] private TMP_Text ammoInventoryText;

    public void UpdateAmmo(int currentAmmo, int inventoryAmmo)
    {
        ammoCurrentText.text = currentAmmo.ToString();
        ammoInventoryText.text = $"/ {inventoryAmmo}";
    }
}
