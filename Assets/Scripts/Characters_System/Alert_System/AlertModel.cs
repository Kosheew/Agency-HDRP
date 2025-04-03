using System;
using UnityEngine;

namespace Alert_System
{
    public class AlertModel
    {
        private float _alertLevel = 0f; 
        public readonly float maxAlertLevel = 1f;
        private readonly float _alertDecayRate = 0.1f;
        private readonly float _alertDecayDelay = 1f;
        private float _timeSinceLastIncrease = 0f;
        
        public event Action<float> OnAlertChange;
        public event Action OnAlertMaxReached;

        public void IncreaseAlert(float alertAmount)
        {
            _alertLevel += alertAmount;
            _alertLevel = Mathf.Clamp(_alertLevel, 0, maxAlertLevel);

            _timeSinceLastIncrease = 0f; 
            
            OnAlertChange?.Invoke(_alertLevel);
            
            if (_alertLevel >= maxAlertLevel)
            {
                OnAlertMaxReached?.Invoke();
            }
        }
        
        public void DecreaseAlert(float deltaTime)
        {
            if (_alertLevel > 0)
            {
                _timeSinceLastIncrease += deltaTime;

                if (_timeSinceLastIncrease >= _alertDecayDelay)
                {
                    _alertLevel -= _alertDecayRate * deltaTime;
                    _alertLevel = Mathf.Max(_alertLevel, 0);
                    OnAlertChange?.Invoke(_alertLevel);
                }
            }
        }

        public void ResetAlertLevel()
        {
            _alertLevel = 0f;
            OnAlertChange?.Invoke(_alertLevel);
        }
    }
}