using Characters;
using InputActions;
using UnityEngine;
using UnityEngine.Events;

namespace Zones
{
    public class TriggerExitDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerExit;
       
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                onTriggerExit?.Invoke();
            }
        }
    }
}