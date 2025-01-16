using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class HealthView : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private float healthChangeDuration = 1f;
        
        public void SetHealth(float health)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }

        public void UpdateHealth(float health)
        {
            StartCoroutine(UpdateHealthSmoothly(health));
        }
        
        private IEnumerator UpdateHealthSmoothly(float targetHealth)
        {
            float startHealth = healthSlider.value;
            float elapsedTime = 0f;

            while (elapsedTime < healthChangeDuration)
            {
                healthSlider.value = Mathf.Lerp(startHealth, targetHealth, elapsedTime / healthChangeDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            healthSlider.value = targetHealth;
        }
    }
}