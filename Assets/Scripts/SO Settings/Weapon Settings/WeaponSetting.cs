using UnityEngine;
using UnityEngine.Serialization;

namespace WeaponSettings
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "ScriptableObjects/Weapons", order = 0)]
    public class WeaponSetting : ScriptableObject
    {
        [Header("Main Settings Weapons")] 
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private float timeReload;
        
        [Header("Interval Settings")] 
        [SerializeField] float intervalBetweenShoots;

        [SerializeField] private float takingTime;
        [SerializeField] private float spreadIncreaseInterval;

        [FormerlySerializedAs("walkingRangeSpread")]
        [Header("Spread Radius Settings")] 
        [Range(0f, 0.4f)] [SerializeField] private float idleRadiusSpread;
        [Range(0.5f, 1f)] [SerializeField] private float walkingRadiusSpread;
        [Range(1f, 1.5f)][SerializeField] private float runningRadiusRadiusSpread;

        [Header("Spread Increase")]
        [Range(1f, 2f)][SerializeField] private float spreadIncreaseMultiplier;
        [Range(1f, 2f)][SerializeField] private float spreadIncreaseRate;
        
        [Header("Ammo Settings")] 
        [SerializeField] private int maxAmmo;
        
        [Header("Clips")] 
        [SerializeField] protected AudioClip shootingSound;
        [SerializeField] protected AudioClip noBulletsSound;
        [SerializeField] protected AudioClip reloadingSound;

        public float Range => range;
        public float Damage => damage;
        public float IntervalBetweenShoots => intervalBetweenShoots;
        public float TakingTime => takingTime;
        public float SpreadIncreaseInterval => spreadIncreaseInterval;
        public float WalkingRadiusSpread => walkingRadiusSpread;
        public float RunningRadiusSpread => runningRadiusRadiusSpread;
        public float SpreadIncreaseMultiplier => spreadIncreaseMultiplier;
        public float SpreadIncreaseRate => spreadIncreaseRate;
        public float IdleRadiusSpread => idleRadiusSpread;
        public int MaxAmmo => maxAmmo;
        public float TimeReload => timeReload;
        
        public AudioClip ShootingSound => shootingSound;
        public AudioClip NoBulletsSound => noBulletsSound;
        public AudioClip ReloadingSound => reloadingSound;
    }
}