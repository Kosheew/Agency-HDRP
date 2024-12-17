using System;
using UnityEngine;

namespace Paused
{
    public class PauseModel
    {
        private bool _isPaused;
        
        public void SetPaused()
        { 
            _isPaused = !_isPaused;
            
            Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Time.timeScale = _isPaused ? 0 : 1; 
            
            OnPauseStateChanged?.Invoke(_isPaused);
        }

        public event Action<bool> OnPauseStateChanged;
    }
}