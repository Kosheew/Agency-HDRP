using System;
using System.Threading;
using UnityEngine;

namespace Timer
{
     public class TimerModel
    {
        private float _time = 0f;
        public bool IsRunning { get; private set; }
        public event Action<int> OnTimeChanged;

        public void StartTimer()
        {
            IsRunning = true;
        }

        public void StopTimer()
        {
            IsRunning = false;
        }

        public void ResetTimer()
        {
            _time = 0f;
            OnTimeChanged?.Invoke(0);
        }

        public void UpdateTimer(float deltaTime)
        {
            if (!IsRunning) return;

            _time += deltaTime;
            OnTimeChanged?.Invoke(Mathf.FloorToInt(_time));
        }
    }
}