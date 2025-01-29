using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Weapons;

public class HolsterController : MonoBehaviour
{
    [SerializeField] private Transform holsterRifleTransform;
    [SerializeField] private Transform holsterPistolTransform;
    
    [SerializeField] private Transform handTransform;

    private Weapon _currentWeapon;
    private Weapon _previousWeapon;
    private bool _isSameTypeSwap;

    public void EquipNewWeapon(Weapon newWeapon)
    {
        _isSameTypeSwap = _currentWeapon != null && _currentWeapon.AnimType == newWeapon.AnimType;
        
        _previousWeapon = _currentWeapon;
        if (_previousWeapon == null) _previousWeapon = newWeapon;
        _currentWeapon = newWeapon;
    }

    public void EquipWeapon()
    {
        if (_currentWeapon == null) return;

        _currentWeapon.gameObject.SetActive(true);
        _currentWeapon.transform.SetParent(handTransform);
        _currentWeapon.transform.localPosition = new Vector3(0.147f, -0.04f, 0.034f);
        _currentWeapon.transform.localRotation = Quaternion.Euler(-2.2f, 98.472f, 88.77f);
    }

    public void UnEquipWeapon()
    {
        if (_previousWeapon == null) return;

        Transform holsterTransform = _previousWeapon.AnimType == WeaponAnimType.Pistol ? holsterPistolTransform : holsterRifleTransform;
        
        _previousWeapon.transform.SetParent(holsterTransform);
        _previousWeapon.transform.localPosition = new Vector3(-0.15f, 0.18f, 0.078f);
        _previousWeapon.transform.localRotation = Quaternion.Euler(-8.1f, 127.879f, 89.256f);

        if (_isSameTypeSwap)
        {
            _previousWeapon.gameObject.SetActive(false);
        }
    }
}
