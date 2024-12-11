using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    private Animator animator;
    private CharacterController characterController;
    private int _hashHorizontal;
    private int _hashVertical;
    
    private bool _running;
    private bool _isCrouched;
    
    private Vector2 lastDirection = Vector2.down; // Початкова сторона, наприклад, вниз.
    private Vector2 moveDirection;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        _hashHorizontal = Animator.StringToHash("Horizontal");
        _hashVertical = Animator.StringToHash("Vertical");
    }

    private void Update()
    {
        float horizontal = Input.GetAxis(Horizontal);
        float vertical = Input.GetAxis(Vertical);
        
        moveDirection = new Vector2(horizontal, vertical);
        
        float rawHorizontal = Input.GetAxisRaw("Horizontal");
        float rawVertical = Input.GetAxisRaw("Vertical");
        
        if (rawHorizontal != 0 || rawVertical != 0)
        {
            lastDirection = moveDirection.normalized;
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            _isCrouched = !_isCrouched;
            animator.SetTrigger("Crouched");
        }
        
        animator.SetBool("IsCrouched", _isCrouched);
        // Передаємо дані в Animator
        animator.SetFloat(_hashHorizontal, moveDirection.x);
        animator.SetFloat(_hashVertical, moveDirection.y);
        animator.SetFloat("LastMoveX", lastDirection.x);
        animator.SetFloat("LastMoveY", lastDirection.y);
        
        
       // animator.SetFloat(_hashHorizontal, horizontal);
       // animator.SetFloat(_hashVertical, vertical);
        
        characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime);
    }
}
