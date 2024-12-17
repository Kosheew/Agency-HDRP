using UnityEngine;
using TMPro;

namespace Timer
{
    public class TimerView : MonoBehaviour
    {
         [SerializeField] private TMP_Text timerText;

        public void UpdateTimerText(int seconds)
        {
            var minutes = seconds / 60;
            var remainingSeconds = seconds % 60;
            timerText.SetText($"{minutes:00}:{remainingSeconds:00}");
        }
    }
}