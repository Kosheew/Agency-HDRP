using Characters.Character_Interfaces;
using UnityEngine;
using WeaponSettings;
using System;

namespace Weapons
{
    [RequireComponent(typeof(AudioSource))]
    //[RequireComponent(typeof(Animator))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponSetting weaponSetting;
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected LayerMask _targetLayer;
        
        [Header("Effects")] 
        [SerializeField] protected ParticleSystem _shootingEffect;
        [SerializeField] protected ParticleSystem _bulletEffect;

        [SerializeField] protected int ammoInventory;
        [SerializeField] protected int currentAmmo;
        
        protected int _maxAmmoStore;
        protected AudioSource _audioSource;
        protected Animator _animator;

        protected float _maxRadiusSpread;
        protected float _lastTimeShoot;
        protected float _lastTimeReload;
        public bool CanShoot { get; set; }
        protected bool _reloading;

        public float DamageValue { get; private set; }
        public float SpreadMultiplier { get; protected set; }
        
        public event Action<int, int> OnAmmoUsed;
        
        public virtual void Init()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            
            _maxAmmoStore = weaponSetting.MaxAmmoStore;
            DamageValue = weaponSetting.Damage;

            CanShoot = false;
        }

        public virtual void Shoot()
        {
            
                GetShoot();
            
        }
        
        public virtual bool CheckShoot()
        {
            if(!CanShoot) return false;
            
            if (Time.time - _lastTimeShoot < weaponSetting.IntervalBetweenShoots) return false;
            
            _lastTimeShoot = Time.time;
            
            if (currentAmmo <= 0)
            {
                if (weaponSetting.NoBulletsSound != null)
                {
                    _audioSource.PlayOneShot(weaponSetting.NoBulletsSound);
                }
                Reload();
                return false;
            }
            
            if (_shootingEffect != null)
            {
                _shootingEffect.Play();
            }

            if (_bulletEffect != null)
            {
                _bulletEffect.Play();
            }

            if (weaponSetting.ShootingSound != null)
            {
                _audioSource.PlayOneShot(weaponSetting.ShootingSound);
            }
            
            currentAmmo--;
            
            OnAmmoUsed?.Invoke(currentAmmo, ammoInventory);
            return true;
        }

        protected virtual void GetShoot()
        {
            Vector3 shootDirection = GetShootDirection();

            var ray = new Ray(_spawnPoint.position, shootDirection);

            if (Physics.Raycast(ray, out var hit, weaponSetting.Range, _targetLayer))
            {
                Debug.Log(hit.collider.gameObject.name);
                HandleHit(hit);
            }
            
            Debug.DrawRay(_spawnPoint.position, shootDirection * weaponSetting.Range, Color.red);
        }

        protected virtual Vector3 GetShootDirection()
        {
            float range = weaponSetting.Range;

            _maxRadiusSpread *= SpreadMultiplier;

            float maxSpreadAngle = Mathf.Atan(_maxRadiusSpread / range);

            Vector2 randomPointInCircle = UnityEngine.Random.insideUnitCircle * Mathf.Tan(maxSpreadAngle) * range;

            Vector3 spreadDirection = new Vector3(randomPointInCircle.x, randomPointInCircle.y, range);

            // Debug.Log("SpreadMultiplier: " + SpreadMultiplier.ToString());
//            Debug.Log("Max Radius Spread: " + _maxRadiusSpread.ToString());

            // Debug.DrawRay(_spawnPoint.position, shootDirection * weaponSetting.Range, Color.blue, 2f);


            return (_spawnPoint.forward + _spawnPoint.TransformDirection(spreadDirection)).normalized;
        }
        
        protected virtual void HandleHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IHealthCharacter healthCharacter))
            {
                var damageCalculator = new DamageCalculatorSimple(this);
                var damage = damageCalculator.CalculateDamage();
                healthCharacter.CharacterHealth.TakeDamage(damage);
            }
        }

        public virtual void SetSpread(float targetSpeed)
        {
            if (targetSpeed >= 2.5f)
            {
                _maxRadiusSpread = weaponSetting.RunningRadiusSpread;
            }
            else if (targetSpeed >= 0.5f)
            {
                _maxRadiusSpread = weaponSetting.WalkingRadiusSpread;
            }
            else
            {
                _maxRadiusSpread = weaponSetting.IdleRadiusSpread;
            }
        }

        public virtual void IncreaseSpread()
        {
            SpreadMultiplier = Mathf.Min(SpreadMultiplier + weaponSetting.SpreadIncreaseRate * Time.deltaTime,
                weaponSetting.SpreadIncreaseMultiplier);
        }

        public virtual void ResetSpread()
        {
            SpreadMultiplier = 0f;
        }
        
        public virtual void Reload()
        {
            if(_reloading || currentAmmo == _maxAmmoStore || ammoInventory <= 0) return;
            
            _reloading = true;
            
         //   _animator?.SetTrigger("Reload");
         
            Invoke(nameof(CompleteReload), weaponSetting.TimeReload);
        }

        private void CompleteReload()
        {
            int ammoNeeded = _maxAmmoStore - currentAmmo;
            int ammoToReload = Mathf.Min(ammoInventory, ammoNeeded);

            currentAmmo += ammoToReload;
            ammoInventory -= ammoToReload;
            
            _audioSource.PlayOneShot(weaponSetting.ReloadingSound);
            OnAmmoUsed?.Invoke(currentAmmo, ammoInventory);
            
            _reloading = false;
           // CancelInvoke(nameof(CompleteReload));
        }

        public bool CheckReload()
        {
            return _reloading;
        }
        
        public void AddAmmo(int ammoToAdd, TypeWeapon typeAmmo)
        {
            if (weaponSetting.TypeWeaponWeapon.Equals(typeAmmo))
            {
                ammoInventory += ammoToAdd;
            }
        }
        
        public int CurrentAmmo => currentAmmo;
        public int AmmoInventory => ammoInventory;
        public Sprite IconWeapon => weaponSetting.IconWeapon;
        public WeaponAnimType AnimType => weaponSetting.AnimType;
    }
}
