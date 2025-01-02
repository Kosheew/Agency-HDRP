using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator 
{
    private readonly Animator _animator;
    private readonly int _animIDSpeed;
    
    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
        
        _animIDSpeed = Animator.StringToHash("Speed");
    }

    public void Attacking()
    {
        // _animator.SetTrigger(_attackHash);
    }

    public void Running(float velocity)
    {
        _animator.SetFloat(_animIDSpeed, velocity);
    }
}
