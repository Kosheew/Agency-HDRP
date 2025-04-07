using System.Collections;
using UnityEngine;

namespace Health_System
{
    public class HealthRegenerationComponent: MonoBehaviour
    {
        [SerializeField] private float regenRate;
        [SerializeField] private float regenDelay;

        private HealthComponent _health;
        private Coroutine _regenCoroutine;
        
        private float _lastDamageTime;

        public void Init()
        {
            _health = GetComponent<HealthComponent>();
            _health.OnHealthChanged.AddListener(OnDamageTaken); 
        }
        
        private void OnDamageTaken(float health)
        {
            _lastDamageTime = Time.time; 
            
            if (_regenCoroutine != null)
            {
                StopCoroutine(_regenCoroutine);
                _regenCoroutine = null;
            }

            StartRegeneration();
        }

        public void StartRegeneration()
        {
            if (_regenCoroutine == null)
            {
                _regenCoroutine = StartCoroutine(RegenerateHealth());
            }
        }

        private IEnumerator RegenerateHealth()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f); 

                if (Time.time - _lastDamageTime >= regenDelay)
                {
                    _health.Heal(regenRate * 0.5f);
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