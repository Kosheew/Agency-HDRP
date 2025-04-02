using Characters.Player;
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
            if (other.TryGetComponent(out PlayerContext player))
            {
                onTriggerExit?.Invoke();
            }
        }
    }
}