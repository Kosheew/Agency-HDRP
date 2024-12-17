using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator 
{
    private readonly Animator _animator;
    private readonly int _attackHash;
    private readonly int _runHash;
    
    public CharacterAnimator(Animator animator)
    {
        _animator = animator;
        _attackHash = Animator.StringToHash("Attack");
        _runHash = Animator.StringToHash("Run");
    }

    public void Attacking()
    {
        _animator.SetTrigger(_attackHash);   
    }

    public void Running(float velocity)
    {
        _animator.SetFloat(_runHash, velocity);
    }
}
