using UnityEngine;

namespace Enemy.Command
{

    /*public class RotateTowardsCommand : ICommand
    {
        private Transform _transform;
        private Vector3 _target;
        private float _rotationSpeed;

        public RotateTowardsCommand(Transform transform, Vector3 target, float rotationSpeed)
        {
            _transform = transform;
            _target = target;
            _rotationSpeed = rotationSpeed;
        }
        
        public void Execute()
        {
            Vector3 direction = (_target - _transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);
        }
    }*/
}
