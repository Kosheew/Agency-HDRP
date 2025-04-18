using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator 
{
    private readonly Animator _animator;
    private readonly int _animIDSpeed;
    private readonly int _animIDAttack;
    private readonly int _animIDChase;
    private readonly int _animIDDeath;
    
    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
        
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDAttack = Animator.StringToHash("Attack");
        _animIDChase = Animator.StringToHash("Chasing");
        _animIDDeath = Animator.StringToHash("Death");
    }

    public void Attacking(bool isAttacking)
    {
         _animator.SetBool(_animIDAttack, isAttacking);
    }

    public void Chasing(bool isChasing)
    {
        _animator.SetBool(_animIDChase, isChasing);
    }
    
    public void Running(float velocity)
    {
        _animator.SetFloat(_animIDSpeed, velocity);
    }

    public void Death()
    {
        _animator.SetTrigger(_animIDDeath);
    }
}
