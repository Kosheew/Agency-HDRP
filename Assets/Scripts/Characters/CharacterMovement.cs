using System;
using UnityEngine;

namespace Characters
{
    public class CharacterMovement : IMovement
    {
        private const float GravityAcceleration = -9.8f;
        
        private readonly float _maxClamp; 
        private readonly float _turnSpeed;
        private readonly float _accelerationRate;
        
        private float _currentAngleX = 0f;
        private float _speedScale;
        
        public CharacterMovement(float maxClamp, float speedScale, float accelerationRate, float turnSpeed)
        {
            _maxClamp = maxClamp;
            _speedScale = speedScale;
            _accelerationRate = accelerationRate;
            _turnSpeed = turnSpeed;
        }
        
        public void AddAcceleration()
        {
            _speedScale += _accelerationRate;
        }
        
        public void SubAcceleration()
        {
            _speedScale = Mathf.Max(0, _speedScale - _accelerationRate);
        }
        
        public void RotateCharacter(float lookX, float lookY, Transform bodyTransform, Transform camera)
        {
            bodyTransform.Rotate(new Vector3(0f, lookX * _turnSpeed * Time.deltaTime, 0f));

            _currentAngleX += lookY * _turnSpeed * Time.deltaTime * -1f;
            _currentAngleX = Mathf.Clamp(_currentAngleX, -_maxClamp, _maxClamp);

            camera.localEulerAngles = new Vector3(_currentAngleX, 0f, 0f);
        }
        
        public Vector3 MoveCharacter(float horizontal, float vertical, Transform bodyTransform)
        {
            var velocity = new Vector3(horizontal, 0, vertical);
            
            return bodyTransform.TransformDirection(velocity) * (_speedScale * Time.deltaTime);
        }

        public Vector3 AddGravity()
        {
            return new Vector3(0f, GravityAcceleration, 0f) * Time.deltaTime;
        }
    }
}
