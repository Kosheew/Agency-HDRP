using Characters;
using UnityEngine;
using Views;
using Weapons;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private AmmoView ammoView;
    [SerializeField] private Weapon[] weapons;

    private IPlayer player;
    
    private int _currentWeapon = 0;

    private AmmoModel ammoModel;
    
    public void Inject(DependencyContainer container)
    {
        ammoModel = new AmmoModel();
        ammoModel.OnAmmoChanged += ammoView.UpdateAmmo;
        
        player = container.Resolve<IPlayer>();

        foreach (var weapon in weapons)
        {
            weapon.Init();
            weapon.OnAmmoUsed += (currentAmmo, ammoInventory) =>
            {
                UpdateAmmo(currentAmmo, ammoInventory);
            };
        }

        player.Weapon = weapons[_currentWeapon];
        
        SelectWeapon(_currentWeapon);
    }

    public void SelectWeapon(int weaponIndex)
    {
        _currentWeapon = weaponIndex;

        var selectedWeapon = weapons[_currentWeapon];
        ammoModel.CurrentAmmo = selectedWeapon.CurrentAmmo;
        ammoModel.AmmoInventory = selectedWeapon.AmmoInventory;
        ammoView.UpdateAmmoIcon(selectedWeapon.IconWeapon);
    }
    
    public void UpdateAmmo(int currentAmmo, int ammoInventory)
    {
        ammoModel.CurrentAmmo = currentAmmo;
        ammoModel.AmmoInventory = ammoInventory;
    }
}
