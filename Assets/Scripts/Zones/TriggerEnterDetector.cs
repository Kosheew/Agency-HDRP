using Characters;
using UnityEngine;
using UnityEngine.Events;

namespace Zones
{
    public class TriggerEnterDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPlayer player))
            {
                onTriggerEnter?.Invoke();
            }
        }
    }
}
