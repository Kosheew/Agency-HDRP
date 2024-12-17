using UnityEngine;

namespace Characters
{
    public class CharacterTouch : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out ITouching objectTouch))
            {
                objectTouch.Touch();
            }
        }
    }
}
