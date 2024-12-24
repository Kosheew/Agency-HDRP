using CharacterSettings;
using Characters;
using InputActions;
using UnityEngine;

namespace Player.State
{
    public abstract class BasePlayerState : IPlayerState
    {
        private PlayerSetting _playerSetting;
        private CharacterController _controller;
        private UserInput _userInput;
        protected PlayerAnimation _playerAnimation;
        
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        
        private bool _isGrounded;
        private bool _isCrouching;
        
        public virtual void EnterState(IPlayer player)
        { 
            _playerSetting = player.PlayerSetting;
            _controller = player.Controller;
            _userInput = player.UserInput;
            _playerAnimation = player.PlayerAnimation;
        }

        public virtual void UpdateState(IPlayer player)
        {
            JumpAndGravity();
            GroundedCheck();
            Crouch();
            Move(player.TransformMain);
            Debug.Log("Base State Entered");
        }

        public virtual void ExitState(IPlayer player)
        {
          
        }
        
        private void GroundedCheck()
        {
            _isGrounded = _controller.isGrounded;
            _playerAnimation.SetGrounded(_isGrounded);
        }
        
        private void Move(Transform transform)
        {
            float targetSpeed = !_isCrouching && _userInput.Sprint   ? _playerSetting.SprintSpeed : _playerSetting.MoveSpeed;
            
            if (_userInput.Move == Vector2.zero) targetSpeed = 0.0f;
            
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.2f;
            float inputMagnitude = _userInput.AnalogMovement ? _userInput.Move.magnitude : 1f;
            
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * _playerSetting.SpeedChangeRate);
                
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * _playerSetting.SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            
            if (Mathf.Abs(_animationBlend - targetSpeed) < 0.01f)
            {
                _animationBlend = targetSpeed;
            }
            
            Vector3 inputDirection = new Vector3(_userInput.Move.x, 0.0f, _userInput.Move.y).normalized;
            
            if (_userInput.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    _playerSetting.RotationSmoothTime);
                
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            
            _playerAnimation.MovementAnim(_animationBlend, inputMagnitude);
        }
        
        private void JumpAndGravity()
        {
            if (_isGrounded)
            {
                _playerAnimation.SetFreeFall(false);

                if (_verticalVelocity < 0)
                {
                    _verticalVelocity = -2;
                }
               
                if (_userInput.Jump )
                {
                    _verticalVelocity = Mathf.Sqrt(_playerSetting.JumpHeight * -2f * _playerSetting.Gravity);
                }
            }
            else
            {
                _playerAnimation.SetFreeFall(true);
            }
            
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _playerSetting.Gravity * Time.deltaTime;
            }
        }
        
        public void Crouch()
        {
            if (_userInput.Crouch && _isGrounded) 
            {
                _isCrouching = !_isCrouching; 
                _playerAnimation.SetCrouched(_isCrouching);
            }
        }
        
        private bool IsMoving(Vector3 velocity)
        {
            var velocityMagnitude = new Vector3(velocity.x, 0, velocity.z).magnitude;
            return velocityMagnitude > 0.01f;
        }
    }
}