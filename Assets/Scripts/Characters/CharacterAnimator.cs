using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator 
{
    private readonly Animator _animator;
    private readonly int _animIDSpeed;
    private readonly int _animIDAttack;
    
    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
        
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAttack = Animator.StringToHash("Attack");
    }

    public void Attacking(bool isAttacking)
    {
         _animator.SetBool(_animIDAttack, isAttacking);
    }

    public void Running(float velocity)
    {
        _animator.SetFloat(_animIDSpeed, velocity);
    }
}
