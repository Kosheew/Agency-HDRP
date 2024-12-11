using UnityEngine;

public class TestAnimator : MonoBehaviour
{
    private const string Vertical = "Vertical";
    private const string Horizontal = "Horizontal";
    
    private Animator animator;

    private int _hashHorizontal;
    private int _hashVertical;
    
    private bool _running;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        _hashHorizontal = Animator.StringToHash("Horizontal");
        _hashVertical = Animator.StringToHash("Vertical");
    }

    private void Update()
    {
        float horizontal = Input.GetAxis(Horizontal);
        float vertical = Input.GetAxis(Vertical);
        
        animator.SetFloat(_hashHorizontal, horizontal);
        animator.SetFloat(_hashVertical, vertical);
    }
}
