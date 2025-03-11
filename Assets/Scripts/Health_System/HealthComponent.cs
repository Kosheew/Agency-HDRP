using UnityEngine;
using System;

namespace Characters.Health
{
    public class HealthComponent
    {
        public float Health { get; private set; }
        public float MaxHealth {get; private set;}
        
        public event Action<float> OnHealthChanged;
        public event Action OnTakeDamage;
        public event Action OnDeath;
        
        public HealthComponent(float health)
        {
            Health = health;
            MaxHealth = health;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            Health = Mathf.Max(0, Health);
            
            OnHealthChanged?.Invoke(Health);
            OnTakeDamage?.Invoke();
            
            if (Health <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        public void Heal(float heal)
        {
            Health += heal;
            OnHealthChanged?.Invoke(Health);
            if (Health >= MaxHealth)
            {
                Health = MaxHealth;
            }
        }
    }
}
