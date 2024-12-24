using UnityEngine;
using System.Collections;
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;
    
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDCrouched;
    private int _animIDEquipped;
    private int _animIDisCrouched;

    private bool _equipped;
    
    public void Init()
    {
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
    }
    
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDCrouched = Animator.StringToHash("Crouched");
        _animIDEquipped = Animator.StringToHash("Equipped");
        _animIDisCrouched = Animator.StringToHash("isCrouched");
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(_animIDGrounded, isGrounded);
    }

    public void SetJump(bool isJump)
    {
        _animator.SetBool(_animIDJump, isJump);
    }
    
    public void SetFreeFall(bool isFreeFall)
    {
        _animator.SetBool(_animIDFreeFall, isFreeFall);
    }

    public void SetCrouched(bool isCrouched)
    {
        _animator.SetBool(_animIDCrouched, isCrouched);
        if(isCrouched)
            _animator.SetTrigger(_animIDisCrouched);
    }
    
    
    public void MovementAnim(float speed, float motionSpeed)
    {
        _animator.SetFloat(_animIDSpeed, speed);
        _animator.SetFloat(_animIDMotionSpeed, motionSpeed);
    }

    public void SetEquipped(bool equipped)
    {
        _animator.SetBool(_animIDEquipped, equipped);
    
     //   _animator.SetLayerWeight(1, equipped ? 1.0f : 0.0f);
    }
    
    private IEnumerator DisableLayerWeightAfterAnimation()
    {
        AnimatorStateInfo stateInfo;

        do
        {
            stateInfo = _animator.GetCurrentAnimatorStateInfo(1); 
            yield return null; 
        } 
        while (stateInfo.normalizedTime < 1f || !stateInfo.IsName("Rifle Put Away"));

        _animator.SetLayerWeight(1, 0);
    }
}
