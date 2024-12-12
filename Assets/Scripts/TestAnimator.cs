using System;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestAnimator : MonoBehaviour
{
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";

    [SerializeField] private float MoveSpeed = 2.0f;
    [SerializeField] private float SprintSpeed = 5.335f;
    
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;
    
    
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    
    private Animator animator;
    private CharacterController characterController;
    private int _hashHorizontal;
    private int _hashVertical;
    
    private bool _running;
    private bool _isCrouched;
    private float deadZone = 0.1f;
    private Vector2 lastDirection = Vector2.down; // Початкова сторона, наприклад, вниз.
    private Vector2 moveDirection;
    
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private BundleInputs _input;
    
    
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        _hashHorizontal = Animator.StringToHash("Horizontal");
        _hashVertical = Animator.StringToHash("Vertical");
        _input = GetComponent<BundleInputs>();

        _verticalVelocity = -9.81f;
    }

    private void Update()
    {
      /*  float horizontal = Input.GetAxis(Horizontal);
        float vertical = Input.GetAxis(Vertical);
        
        moveDirection = new Vector2(horizontal, vertical);
        
        float rawHorizontal = Input.GetAxisRaw("Horizontal");
        float rawVertical = Input.GetAxisRaw("Vertical");
        
        
        animator.SetBool("IsMoving", moveDirection.magnitude >= 0.5f);
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouched = !_isCrouched;
            animator.SetTrigger("Crouched");
        }
        
        animator.SetBool("IsCrouched", _isCrouched);
        // Передаємо дані в Animator
        
        float moveX = Mathf.Abs(moveDirection.x) > deadZone ? moveDirection.x : 0;
        float moveY = Mathf.Abs(moveDirection.y) > deadZone ? moveDirection.y : 0;
        
        if (moveDirection.magnitude >= 0.1f)
        {
            lastDirection = moveDirection.normalized;
        }
        
        animator.SetFloat(_hashHorizontal, moveX);
        animator.SetFloat(_hashVertical, moveY);
        animator.SetFloat("LastMoveX", lastDirection.x);
        animator.SetFloat("LastMoveY", lastDirection.y);
        
        
        
       // animator.SetFloat(_hashHorizontal, horizontal);
       // animator.SetFloat(_hashVertical, vertical);*/
        
      float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
      
      if (_input.move == Vector2.zero) targetSpeed = 0.0f;
      
      float currentHorizontalSpeed = new Vector3(characterController.velocity.x, 0.0f, characterController.velocity.z).magnitude;

      float speedOffset = 0.1f;
      float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;
      
      if (currentHorizontalSpeed < targetSpeed - speedOffset ||
          currentHorizontalSpeed > targetSpeed + speedOffset)
      {
          // creates curved result rather than a linear one giving a more organic speed change
          // note T in Lerp is clamped, so we don't need to clamp our speed
          _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
              Time.deltaTime * SpeedChangeRate);

          // round speed to 3 decimal places
          _speed = Mathf.Round(_speed * 1000f) / 1000f;
      }
      else
      {
          _speed = targetSpeed;
      }
      _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
      if (_animationBlend < 0.01f) _animationBlend = 0f;

      // normalise input direction
      Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

      // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
      // if there is a move input rotate player when the player is moving
      if (_input.move != Vector2.zero)
      {
          _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
          float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
              RotationSmoothTime);

          // rotate to face input direction relative to camera position
          transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
      }
      
      Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

      // move the player
      characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                       new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

      Debug.Log(targetDirection.normalized);
      // update animator if using character
     // if (_hasAnimator)
      {
          animator.SetFloat("Speed", _animationBlend);
          animator.SetFloat("MotionSpeed", inputMagnitude);
      }
      
    }
    
    
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        /* if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }*/
    }
    /*
         private void JumpAndGravity()
        {
            if (Grounded)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    */
}
