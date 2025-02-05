using Characters;
using InputActions;
using UnityEngine;
using UnityEngine.Events;

namespace Zones
{
    public class TriggerUseDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent onUsePressed;

        private UserInput _userInput;
        private bool _playerInside;

        public void Inject(DependencyContainer container)
        {
            _userInput = container.Resolve<UserInput>();
        }

        public void SetPlayerInside(bool value)
        {
            _playerInside = value;
            Debug.Log(_playerInside);
        }

        public void LateUpdateState()
        {
            if (_playerInside && _userInput.Use)
            {
                onUsePressed?.Invoke();
                Debug.Log("Use Pressed");
            }
        }
    }
}