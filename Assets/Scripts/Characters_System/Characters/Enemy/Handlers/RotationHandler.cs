using UnityEngine;

namespace CustomAI.Handlers
{
    public class RotationHandler: MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float facingAngleThreshold;
        
        public void HandleRotation(Vector3 target)
        {
            
            Vector3 targetDirection = (target - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, targetDirection);

            if (angle > facingAngleThreshold)
            {
                RotateTowards(targetDirection);
                Debug.Log("Rotate");
            }
        }
        
        private void RotateTowards(Vector3 direction)
        {
            direction.y = 0; 
            if (direction == Vector3.zero) return;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }
}