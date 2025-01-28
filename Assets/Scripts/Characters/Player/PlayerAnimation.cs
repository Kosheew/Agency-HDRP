using UnityEngine;
using System.Collections;
using Weapons;

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
    private int _animIDDeath;
    private int _animIDRifle;
    private int _animIDPistol;
    private int _animIDHit;
    private int _animIDChangeWeapon;
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
        _animIDDeath = Animator.StringToHash("Death");
        _animIDHit = Animator.StringToHash("Hit");
        _animIDRifle = Animator.StringToHash("Rifle");
        _animIDPistol = Animator.StringToHash("Pistol");
        _animIDChangeWeapon = Animator.StringToHash("Change Weapon");
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
    }

    public void Death()
    {
        _animator.SetTrigger(_animIDDeath);
    }

    public void ChangeWeapon(Weapon weapon)
    {
        switch (weapon.AnimType)
        {
            case WeaponAnimType.Pistol:
                _animator.SetBool(_animIDPistol, true);
                _animator.SetBool(_animIDRifle, false);
                break;

            case WeaponAnimType.Rifle:
                
                _animator.SetBool(_animIDPistol, false);
                _animator.SetBool(_animIDRifle, true);
                break;
        }
        
        _animator.SetTrigger(_animIDChangeWeapon);
    }
}
