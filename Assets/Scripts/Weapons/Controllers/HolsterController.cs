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

    public void EquipNewWeapon(Weapon newWeapon)
    {
        bool isSameTypeSwap = _currentWeapon != null && _currentWeapon.AnimType == newWeapon.AnimType;
        if (_currentWeapon != null)
        {
            UnEquipWeapon(_currentWeapon, isSameTypeSwap);
        }
        
        _currentWeapon = newWeapon;
        EquipWeapon(_currentWeapon);
    }

    public void EquipWeapon(Weapon weapon)
    {
        weapon.gameObject.SetActive(true);
        weapon.transform.SetParent(handTransform);
        weapon.transform.localPosition = new Vector3(0.147f, -0.04f, 0.034f);
        weapon.transform.localRotation = Quaternion.Euler(-2.2f, 98.472f, 88.77f);
    }

    public void UnEquipWeapon(Weapon weapon, bool disableWeapon)
    {
        Transform holsterTransform = weapon.AnimType == WeaponAnimType.Pistol ? holsterPistolTransform : holsterRifleTransform;
        
        weapon.transform.SetParent(holsterTransform);
        weapon.transform.localPosition = new Vector3(-0.15f, 0.18f, 0.078f);
        weapon.transform.localRotation = Quaternion.Euler(-8.1f, 127.879f, 89.256f);
        
        if (disableWeapon)
        {
            weapon.gameObject.SetActive(false);
        }
    }
}
