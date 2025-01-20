using Characters.Enemy;
using UnityEngine;

namespace Weapons
{
    public class Pistol : Weapon
    {
        [SerializeField] protected int _bullets;
        [SerializeField] private float _reloadingTime;

        private bool _canReload = true;

        public override void Shoot()
        {
            base.Shoot();
            /*if ((_maxAmmo > 0 && _canShoot) || weaponSetting.IsEnemy)
            {


                _maxAmmo--;
                Debug.Log("piu");

                _lastTimeShoot = Time.time;
            }
            else if (_maxAmmo <= 0)
            {
                Debug.Log("no piu");
                //Play no bullets sound
            }*/
        }

        public void StartReloading()
        {
            _canReload = false;
            _canShoot = false;
            Invoke(nameof(Reload), _reloadingTime);
        }

      

    }
}