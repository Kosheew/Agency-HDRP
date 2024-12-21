using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using InputActions;

namespace Controllers
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;
        
        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;
        
        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;
        
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        
        private PlayerInput _playerInput;
        private CharacterController _controller;
        private UserInput _input;
        private GameObject _mainCamera;
        
        private PlayerAnimation _playerAnimation;
        
        private bool _crouched;
        private bool _equipped;

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<UserInput>();
            _playerInput = GetComponent<PlayerInput>();
            _playerAnimation = GetComponent<PlayerAnimation>();
            _playerAnimation.Init();
        }

        private void Update()
        {
            JumpAndGravity();
            GroundedCheck();
            Crouch();
            Equip();
            Move();
        }
        
        private void GroundedCheck()
        {
            Grounded = _controller.isGrounded;
           _playerAnimation.SetGrounded(Grounded);
        }

        public void Crouch()
        {
            if (_input.Crouch && Grounded) 
            {
                _crouched = !_crouched; 
                _playerAnimation.SetCrouched(_crouched);
                _input.Crouch = false;
            }
        }
        
        private void Move()
        {
            float targetSpeed = _input.Sprint && !_crouched ? SprintSpeed : MoveSpeed;
            
            if (_input.Move == Vector2.zero) targetSpeed = 0.0f;
            
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.2f;
            float inputMagnitude = _input.AnalogMovement ? _input.Move.magnitude : 1f;
            
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }
            
            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;
            
            if (Mathf.Abs(_animationBlend - targetSpeed) < 0.01f)
            {
                _animationBlend = targetSpeed;
            }
            
            Vector3 inputDirection = new Vector3(_input.Move.x, 0.0f, _input.Move.y).normalized;
            
            if (_input.Move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);
                
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
            
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            
           _playerAnimation.MovementAnim(_animationBlend, inputMagnitude);
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
               _playerAnimation.SetJump(false);
               _playerAnimation.SetFreeFall(false);
                
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }
                
                if (_input.Jump )
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    
                    _playerAnimation.SetJump(true);
                }
            }
            else
            {
                _playerAnimation.SetFreeFall(true);
            }
            
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private void Equip()
        {
            if (_input.Equip)
            {
                _equipped = !_equipped;
                _playerAnimation.SetEquipped(_equipped);
                _input.Equip = false;
            }
        }
    }
}
