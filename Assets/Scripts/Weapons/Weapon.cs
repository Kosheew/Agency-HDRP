using Characters.Character_Interfaces;
using UnityEngine;
using WeaponSettings;

namespace Weapons
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected WeaponSetting weaponSetting;
        [SerializeField] protected Transform _spawnPoint;
        [SerializeField] protected LayerMask _targetLayer;
        
        [Header("Effects")] 
        [SerializeField] protected ParticleSystem _shootingEffect;
        [SerializeField] protected ParticleSystem _bulletEffect;

        protected AudioSource _audioSource;
        protected Animator _animator;

        private float _maxRadiusSpread;
        protected float _lastTimeShoot;
        protected int _maxAmmo;
        protected bool _canShoot = true;
        public float DamageValue { get; private set; }
        public float SpreadMultiplier { get; private set; }
        
        public virtual void Init()
        {
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            
            _maxAmmo = weaponSetting.MaxAmmo;

            DamageValue = weaponSetting.Damage;

            _canShoot = true;
        }

        public virtual void Shoot()
        {
            if (_maxAmmo <= 0)
            {
                if (weaponSetting.NoBulletsSound != null)
                {
                    _audioSource.PlayOneShot(weaponSetting.NoBulletsSound);
                }

                return;
            }

            if (Time.time - _lastTimeShoot < weaponSetting.IntervalBetweenShoots) return;

            _lastTimeShoot = Time.time;

            Vector3 shootDirection = GetShootDirection();

            var ray = new Ray(_spawnPoint.position, shootDirection);

            if (Physics.Raycast(ray, out var hit, weaponSetting.Range, _targetLayer))
            {
                HandleHit(hit);
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

            _maxAmmo--;

            Debug.DrawRay(_spawnPoint.position, shootDirection * weaponSetting.Range, Color.red);
        }

        private Vector3 GetShootDirection()
        {
            float range = weaponSetting.Range;

            _maxRadiusSpread *= SpreadMultiplier;

            float maxSpreadAngle = Mathf.Atan(_maxRadiusSpread / range);

            Vector2 randomPointInCircle = Random.insideUnitCircle * Mathf.Tan(maxSpreadAngle) * range;

            Vector3 spreadDirection = new Vector3(randomPointInCircle.x, randomPointInCircle.y, range);

            // Debug.Log("SpreadMultiplier: " + SpreadMultiplier.ToString());
            Debug.Log("Max Radius Spread: " + _maxRadiusSpread.ToString());
            
            return (_spawnPoint.forward + _spawnPoint.TransformDirection(spreadDirection)).normalized;
        }
        
        private void HandleHit(RaycastHit hit)
        {
            if (hit.collider.TryGetComponent(out IHealthCharacter healthCharacter))
            {
                var damageCalculator = new DamageCalculatorSimple(this);
                var damage = damageCalculator.CalculateDamage();
                healthCharacter.CharacterHealth.TakeDamage(damage);
            }
        }

        public void SetSpread(float targetSpeed)
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

        public void IncreaseSpread()
        {
            SpreadMultiplier = Mathf.Min(SpreadMultiplier + weaponSetting.SpreadIncreaseRate * Time.deltaTime,
                weaponSetting.SpreadIncreaseMultiplier);
        }

        public void ResetSpread()
        {
            SpreadMultiplier = 0f;
        }
    }
}
