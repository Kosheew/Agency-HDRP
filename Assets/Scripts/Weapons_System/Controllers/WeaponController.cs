using Characters;
using InputActions;
using UnityEngine;
using Views;
using Weapons;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private AmmoView ammoView;
    [SerializeField] private Weapon[] weapons;

    private IPlayer player;
    private int _currentWeaponIndex = 0;

    private bool _isSwitchingWeapon = false;
    
    private PlayerAnimation _playerAnimation;
    private AmmoModel _ammoModel;
    private UserInput _userInput;
    private HolsterController _holsterController;
    private Weapon _previousWeapon;
    
    private Dictionary<Weapon, (Vector3 position, Quaternion rotation)> _weaponInitialTransforms = new();
    
    public void Inject(DependencyContainer container)
    {
        _ammoModel = new AmmoModel();
        _ammoModel.OnAmmoChanged += ammoView.UpdateAmmo;
        
        player = container.Resolve<IPlayer>();
        _userInput = GetComponent<UserInput>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        _holsterController = GetComponent<HolsterController>();
        
        foreach (var weapon in weapons)
        {
            weapon.Init();
            weapon.OnAmmoUsed += (currentAmmo, ammoInventory) =>
            {
                UpdateAmmo(currentAmmo, ammoInventory);
            };
            
            _weaponInitialTransforms[weapon] = (weapon.transform.localPosition, weapon.transform.localRotation);
        }
        
        SelectWeapon(_currentWeaponIndex);
        
        _userInput.OnWeaponScroll += ScrollWeapon;
    }

    public void ScrollWeapon(float scrollValue)
    {
        if (_isSwitchingWeapon) return;
        
        ChangeWeapon((int)Mathf.Clamp(scrollValue, -1, 1));
    }
    
    private void ChangeWeapon(int direction)
    {
        _isSwitchingWeapon = true;
        _currentWeaponIndex = (_currentWeaponIndex + direction + weapons.Length) % weapons.Length;
        SelectWeapon(_currentWeaponIndex);
    }
    
    
    public void SelectWeapon(int weaponIndex)
    {
        var selectedWeapon = weapons[weaponIndex];
        
        bool isSameTypeSwap = _previousWeapon != null && _previousWeapon.AnimType == selectedWeapon.AnimType;
        
        if(_previousWeapon != null) _previousWeapon.CanShoot = false;
        
        _playerAnimation.ChangeWeapon(selectedWeapon, isSameTypeSwap);
        _holsterController.EquipNewWeapon(selectedWeapon);
        player.Weapon = selectedWeapon;
        
        _ammoModel.CurrentAmmo = selectedWeapon.CurrentAmmo;
        _ammoModel.AmmoInventory = selectedWeapon.AmmoInventory;
        ammoView.UpdateAmmoIcon(selectedWeapon.IconWeapon);

        _previousWeapon = selectedWeapon;
        Invoke(nameof(WaitWeaponShoot), 1.6f);
    }
    
    private void WaitWeaponShoot()
    {
        weapons[_currentWeaponIndex].CanShoot = true;
        _isSwitchingWeapon = false;
    }
    
    public void UpdateAmmo(int currentAmmo, int ammoInventory)
    {
        _ammoModel.CurrentAmmo = currentAmmo;
        _ammoModel.AmmoInventory = ammoInventory;
    }
}
