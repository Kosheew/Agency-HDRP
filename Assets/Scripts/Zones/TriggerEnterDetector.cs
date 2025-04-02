using Characters;
using Characters.Player;
using UnityEngine;
using UnityEngine.Events;

namespace Zones
{
    public class TriggerEnterDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent onTriggerEnter;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlayerContext player))
            {
                onTriggerEnter?.Invoke();
            }
        }
    }
}
