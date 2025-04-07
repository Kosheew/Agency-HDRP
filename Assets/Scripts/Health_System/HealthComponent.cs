using UnityEngine;
using System;
using UnityEngine.Events;

namespace Health_System
{
    public class HealthComponent: MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private float _currentHealth;
        
        public float Health => _currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsAlive => _currentHealth > 0;
        
        public UnityEvent<float> OnHealthChanged;
        public UnityEvent OnTakeDamage;
        public UnityEvent OnDeath;

        public void Init()
        {
            _currentHealth = maxHealth;
            
            OnHealthChanged?.Invoke(_currentHealth);
        }
        
        public void TakeDamage(float damage)
        {
            if(!IsAlive || damage <= 0) return;
            
            _currentHealth -= damage;
            _currentHealth = Mathf.Max(0, _currentHealth);
            
            OnHealthChanged?.Invoke(_currentHealth);
            OnTakeDamage?.Invoke();
            
            if (_currentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void Heal(float heal) {
            if (!IsAlive) return;

            _currentHealth = Mathf.Min(maxHealth, _currentHealth + heal);
            OnHealthChanged?.Invoke(_currentHealth);
        }
    }
}
