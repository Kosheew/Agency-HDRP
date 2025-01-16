using System.Collections;
using UnityEngine;

namespace Weapons
{

    public class SubmachineGun : Weapon
    {

        private void OnEnable()
        {
            _canShoot = false;
            Invoke("CanShoot", weaponSetting.TakingTime);
        }

        public override void Shoot()
        {

        }

        public IEnumerator Shootick()
        {
            if (_maxAmmo > 0)
            {
                base.Shoot();

                _maxAmmo--;
                Debug.Log("piu");
                _lastTimeShoot = Time.time;

                yield return new WaitForSeconds(weaponSetting.IntervalBetweenShoots);
                StartCoroutine(Shootick());
            }

            if (_maxAmmo <= 0)
            {
                // Play no bullets sound
                Debug.Log("No piu");
                yield break;
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}