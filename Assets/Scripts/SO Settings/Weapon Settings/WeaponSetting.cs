using UnityEngine;
using UnityEngine.Serialization;
using Weapons;
using Sirenix.OdinInspector;

namespace WeaponSettings
{
    [CreateAssetMenu(fileName = "Weapons", menuName = "ScriptableObjects/Weapons", order = 0)]
    public class WeaponSetting : ScriptableObject
    {
        [Header("Main Settings Weapons")] 
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private float timeReload;
        
        [Header("Weapon Type")]
        [SerializeField] private WeaponAnimType animType;
        [SerializeField] private TypeWeapon typeWeapon;
        
        [ShowIf("@typeWeapon == TypeWeapon.Shotgun")]
        [SerializeField] private int cartageAmount = 0;

        [ShowIf("@typeWeapon == TypeWeapon.Shotgun")] 
        [Range(1f, 10f)] [SerializeField] private float amountSpreadIncreaseRate;
        
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
        
        [FormerlySerializedAs("maxAmmo")]
        [Header("Ammo Settings")] 
        [SerializeField] private int maxAmmoStore;
        
        [Header("Clips")] 
        [SerializeField] private AudioClip shootingSound;
        [SerializeField] private AudioClip noBulletsSound;
        [SerializeField] private AudioClip reloadingSound;

        [SerializeField] private Sprite iconWeapon;
        
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
        public int MaxAmmoStore => maxAmmoStore;
        public float TimeReload => timeReload;
        public int CartageAmount => cartageAmount;
        public float AmountSpreadIncreaseRate => amountSpreadIncreaseRate;
        
        public AudioClip ShootingSound => shootingSound;
        public AudioClip NoBulletsSound => noBulletsSound;
        public AudioClip ReloadingSound => reloadingSound;
        public Sprite IconWeapon => iconWeapon;
        public TypeWeapon TypeWeaponWeapon => typeWeapon;
        public WeaponAnimType AnimType => animType;
    }
}