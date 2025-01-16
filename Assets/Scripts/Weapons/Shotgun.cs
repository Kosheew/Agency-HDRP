using UnityEngine;

namespace Weapons
{
    public class Shotgun : Weapon
    {
        [SerializeField] private int _cartageAmount = 8;

        public override void Shoot()
        {
            if (_maxAmmo > 0)
            {
                _canShoot = false;

                for (int i = 0; i <= _cartageAmount; i++)
                {
                    float spread = 0; //GetSpread();
                    float baseSpread = 1f + spread;

                    Vector3 spreadDirection = new Vector3(
                        Random.Range(-baseSpread, baseSpread),
                        Random.Range(-baseSpread, baseSpread),
                        0
                    );

                    Vector3 shootDirection = (_spawnPoint.forward + _spawnPoint.TransformDirection(spreadDirection))
                        .normalized;

                    RaycastHit hit;
                    if (Physics.Raycast(_spawnPoint.position, shootDirection, out hit, weaponSetting.Range))
                    {
                        //Take damage
                        //Play shooting sound
                        //Play shooting effect
                    }
                }

                _maxAmmo--;
                Debug.Log("Piu");
            }

            if (_maxAmmo <= 0)
            {
                //Play no bullets sound
            }


        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}