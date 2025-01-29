using Characters;
using InputActions;
using UnityEngine;
using Views;
using Weapons;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private AmmoView ammoView;
    [SerializeField] private Weapon[] weapons;

    private IPlayer player;
    private int _currentWeapon = 0;

    private PlayerAnimation _playerAnimation;
    private AmmoModel _ammoModel;
    private UserInput _userInput;
    private HolsterController _holsterController;
    private Weapon _previousWeapon;
    
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
        }
        
        SelectWeapon(_currentWeapon);
        
        _userInput.OnWeaponScroll += ScrollWeapon;
    }

    public void ScrollWeapon(float scrollValue)
    {
        if (scrollValue > 0)
        {
            NextWeapon();
        }
        else if (scrollValue < 0)
        {
            PreviousWeapon();
        }
    }
    
    private void NextWeapon()
    {
        _currentWeapon = (_currentWeapon + 1) % weapons.Length; 
        SelectWeapon(_currentWeapon);
    }

    private void PreviousWeapon()
    {
        _currentWeapon = (_currentWeapon - 1 + weapons.Length) % weapons.Length; 
        SelectWeapon(_currentWeapon);
    }
    
    public void SelectWeapon(int weaponIndex)
    {
        var selectedWeapon = weapons[weaponIndex];
        
        bool isSameTypeSwap = _previousWeapon != null && _previousWeapon.AnimType == selectedWeapon.AnimType;
        
        _playerAnimation.ChangeWeapon(selectedWeapon, isSameTypeSwap);
        _holsterController.EquipWeapon(selectedWeapon);
        player.Weapon = selectedWeapon;
        
        _ammoModel.CurrentAmmo = selectedWeapon.CurrentAmmo;
        _ammoModel.AmmoInventory = selectedWeapon.AmmoInventory;
        ammoView.UpdateAmmoIcon(selectedWeapon.IconWeapon);

        _previousWeapon = selectedWeapon;
    }
    
    public void UpdateAmmo(int currentAmmo, int ammoInventory)
    {
        _ammoModel.CurrentAmmo = currentAmmo;
        _ammoModel.AmmoInventory = ammoInventory;
    }
}
