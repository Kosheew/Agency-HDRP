using CharacterSettings;
using Characters;
using Characters.Player;
using InputActions;
using UnityEngine;

namespace Player.State
{
    public class BasePlayerState : IPlayerState
    {
        private PlayerSetting _playerSetting;
        private CharacterController _controller;
        private UserInput _userInput;
        private PlayerAnimation _playerAnimation;
        
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        private bool _isFreeFall;
        
        public virtual void EnterState(PlayerContext player)
        { 
            _playerSetting = player.PlayerSetting;
            _controller = player.Controller;
            _userInput = player.UserInput;
            _playerAnimation = player.PlayerAnimation;
        }
        
        public virtual void UpdateState(PlayerContext  player)
        {
            GroundedCheck(player);
            JumpAndGravity(player);
            Crouch(player);
            Move(player);
        }

        public virtual void ExitState(PlayerContext  player)
        {
          
        }
        
        private void GroundedCheck(PlayerContext  player)
        {
            bool hasGround = Physics.Raycast(player.TransformMain.position + Vector3.up * 0.1f, Vector3.down, 0.4f);

            player.Grounded = _controller.isGrounded || hasGround;
            
            _playerAnimation.SetGrounded(player.Grounded);
        }
        
        private void Move(PlayerContext  player)
        {
            float targetSpeed = !player.Sneaked && _userInput.Sprint ? _playerSetting.SprintSpeed : _playerSetting.MoveSpeed;

            if (_userInput.Move == Vector2.zero)
            {
                targetSpeed = 0.0f;
                _speed = targetSpeed;
            }
            
            float inputMagnitude = _userInput.AnalogMovement ? _userInput.Move.magnitude : 1f;
            
            _speed = Mathf.Lerp(_speed, targetSpeed * inputMagnitude, Time.deltaTime * _playerSetting.SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
            
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
                float rotation = Mathf.SmoothDampAngle(player.TransformMain.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    _playerSetting.RotationSmoothTime);
                
                player.TransformMain.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            if (!_isFreeFall)
            {
                if (_speed > 0.1f && _speed <= _playerSetting.MoveSpeed)
                {
                    player.FootstepHandler.PlayFootstepWalkSound();
                }
                else if (_speed > _playerSetting.MoveSpeed)
                {
                    player.FootstepHandler.PlayFootstepRunSound();
                }
            }

            _playerAnimation.MovementAnim(_animationBlend, inputMagnitude);
        }
        
        private void JumpAndGravity(PlayerContext  player)
        {
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += _playerSetting.Gravity * Time.deltaTime;
            }
            
            if (player.Grounded)
            {
                if (_isFreeFall) 
                {
                    player.FootstepHandler.PlayFootstepLandSound(); 
                }
                _isFreeFall = false;
                
                if (_verticalVelocity < 0)
                {
                    _verticalVelocity = -2;
                }
               
                if (_userInput.Jump && !player.Sneaked)
                {
                    _verticalVelocity = Mathf.Sqrt(_playerSetting.JumpHeight * -2f * _playerSetting.Gravity);
                    player.FootstepHandler.PlayFootstepJumpSound();
                }
            }
            else
            {
                _isFreeFall = true;
            }
            _playerAnimation.SetFreeFall(_isFreeFall);
        }
        
        private void Crouch(PlayerContext  player)
        {
            if (_userInput.Crouch && player.Grounded)
            {
                player.Sneaked = !player.Sneaked;
                _playerAnimation.SetCrouched(player.Sneaked);
            }
        }
        
        private bool IsMoving(Vector3 velocity)
        {
            var velocityMagnitude = new Vector3(velocity.x, 0, velocity.z).magnitude;
            return velocityMagnitude > 0.01f;
        }
    }
}