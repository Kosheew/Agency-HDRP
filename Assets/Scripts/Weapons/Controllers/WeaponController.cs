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
    
    public void Inject(DependencyContainer container)
    {
        _ammoModel = new AmmoModel();
        _ammoModel.OnAmmoChanged += ammoView.UpdateAmmo;
        
        player = container.Resolve<IPlayer>();
        _userInput = GetComponent<UserInput>();
        _playerAnimation = GetComponent<PlayerAnimation>();
        
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
        var currentWeapon = weapons[_currentWeapon];
        currentWeapon.gameObject.SetActive(false); // Вимикаємо поточну зброю
        _currentWeapon = (_currentWeapon + 1) % weapons.Length; // Наступна зброя
        SelectWeapon(_currentWeapon);
    }

    private void PreviousWeapon()
    {
        var currentWeapon = weapons[_currentWeapon];
        currentWeapon.gameObject.SetActive(false); // Вимикаємо поточну зброю
        _currentWeapon = (_currentWeapon - 1 + weapons.Length) % weapons.Length; // Попередня зброя
        SelectWeapon(_currentWeapon);
    }
    
    public void SelectWeapon(int weaponIndex)
    {
        _currentWeapon = weaponIndex;

        var selectedWeapon = weapons[_currentWeapon];
        selectedWeapon.gameObject.SetActive(true);
        
        _playerAnimation.ChangeWeapon(selectedWeapon);
        player.Weapon = selectedWeapon;
        
        _ammoModel.CurrentAmmo = selectedWeapon.CurrentAmmo;
        _ammoModel.AmmoInventory = selectedWeapon.AmmoInventory;
        ammoView.UpdateAmmoIcon(selectedWeapon.IconWeapon);
    }
    
    public void UpdateAmmo(int currentAmmo, int ammoInventory)
    {
        _ammoModel.CurrentAmmo = currentAmmo;
        _ammoModel.AmmoInventory = ammoInventory;
    }
}
