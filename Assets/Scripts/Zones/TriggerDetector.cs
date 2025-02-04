using Characters;
using InputActions;
using UnityEngine;
using UnityEngine.Events;

public class TriggerDetector : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnter;
    [SerializeField] private UnityEvent onTriggerExit;
    [SerializeField] private UnityEvent onUsePressed; 

    private UserInput _userInput;
    private bool _playerInside; 
    
    public bool PlayerInside => _playerInside;
    
    public void Inject(DependencyContainer container)
    {
        _userInput = container.Resolve<UserInput>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IPlayer player))
        {
            _playerInside = true;
            onTriggerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out IPlayer player))
        {
            _playerInside = false;
            onTriggerExit?.Invoke();
        }
    }

    public void LateUpdateState()
    {
        if (_playerInside && _userInput.Use) 
        {
            onUsePressed?.Invoke(); 
        }
    }
}
