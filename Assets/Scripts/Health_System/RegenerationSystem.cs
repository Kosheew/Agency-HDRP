using System.Collections;
using UnityEngine;

namespace Characters.Health
{
    public class RegenerationSystem
    {
        private readonly HealthComponent _health;
        private readonly float _regenRate;
        private readonly float _regenDelay;
        private readonly MonoBehaviour _coroutineRunner;
        private Coroutine _regenCoroutine;
        private float _lastDamageTime;

        public RegenerationSystem(HealthComponent health, float regenRate, float regenDelay, MonoBehaviour coroutineRunner)
        {
            _health = health;
            _regenRate = regenRate;
            _regenDelay = regenDelay;
            _coroutineRunner = coroutineRunner;
            
            _health.OnHealthChanged += OnDamageTaken; 
        }

        private void OnDamageTaken(float health)
        {
            _lastDamageTime = Time.time; 
            
            if (_regenCoroutine != null)
            {
                _coroutineRunner.StopCoroutine(_regenCoroutine);
                _regenCoroutine = null;
            }

            StartRegeneration();
        }

        public void StartRegeneration()
        {
            if (_regenCoroutine == null)
            {
                _regenCoroutine = _coroutineRunner.StartCoroutine(RegenerateHealth());
            }
        }

        private IEnumerator RegenerateHealth()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f); 

                if (Time.time - _lastDamageTime >= _regenDelay && _health.Health < _health.MaxHealth)
                {
                    _health.Heal(_regenRate * 0.5f);
                }

                if (_health.Health >= _health.MaxHealth)
                {
                    _regenCoroutine = null;
                    yield break; 
                }
            }
        }
    }
}