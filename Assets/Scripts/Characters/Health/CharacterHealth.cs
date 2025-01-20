using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Characters.Health
{
    public class CharacterHealth
    {
        private float _health;

        public event Action<float> OnHealthChanged;
        public event Action OnDeath;
        
        public CharacterHealth(float health)
        {
            _health = health;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
            _health = Mathf.Max(0, _health);
            
            OnHealthChanged?.Invoke(_health);
            Debug.Log(_health.ToString());
            
            if (_health <= 0)
            {
                OnDeath?.Invoke();
            }
        }
    }
}
