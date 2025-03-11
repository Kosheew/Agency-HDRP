using UnityEngine;

namespace Weapons
{
    public class Shotgun : Weapon
    {
        public override void Shoot()
        {
            for (int i = 0; i < weaponSetting.CartageAmount; i++)
            {
                GetShoot();
            }
        }

        /*public override void IncreaseSpread()
        {
            SpreadMultiplier = Mathf.Min(
                SpreadMultiplier + weaponSetting.SpreadIncreaseRate * weaponSetting.AmountSpreadIncreaseRate * Time.deltaTime,
                weaponSetting.SpreadIncreaseMultiplier
                );
        }
        
        protected override Vector3 GetShootDirection()
        {
            float range = weaponSetting.Range;

            float maxSpreadAngle = Mathf.Atan(weaponSetting.AmountSpreadIncreaseRate / range);
            Vector2 randomPointInCircle = Random.insideUnitCircle * Mathf.Tan(maxSpreadAngle) * range;

            Vector3 spreadDirection = new Vector3(
                randomPointInCircle.x,
                randomPointInCircle.y,
                range
            );

            return (FirePoint.forward + FirePoint.TransformDirection(spreadDirection)).normalized;
        }*/
    }
}