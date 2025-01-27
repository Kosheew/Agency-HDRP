using System;

namespace Weapons
{
    public class AmmoModel
    {
        public event Action<int, int> OnAmmoChanged;

        private int _currentAmmo;
        private int _ammoInventory;

        public int CurrentAmmo
        {
            get => _currentAmmo;
            set
            {
                _currentAmmo = value;
                NotifyAmmoChanged();
            }
        }

        public int AmmoInventory
        {
            get => _ammoInventory;
            set
            {
                _ammoInventory = value;
                NotifyAmmoChanged();
            }
        }

        private void NotifyAmmoChanged()
        {
            OnAmmoChanged?.Invoke(_currentAmmo, _ammoInventory);
        }
    }
}