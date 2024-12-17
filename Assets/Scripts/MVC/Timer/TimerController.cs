using UnityEngine;

namespace Timer
{
    public class TimerController
    {
        private readonly TimerView _viewTimer;
        private readonly TimerModel _timerModel;
        
        public TimerController(TimerModel timerModel, TimerView viewTimer)
        {
            _viewTimer = viewTimer;
            _timerModel = timerModel;
            
            _timerModel.OnTimeChanged += UpdateView;
            
            _timerModel.StartTimer();
        }

        private void UpdateView(int time)
        {
            _viewTimer.UpdateTimerText(time);
        }
    }
}