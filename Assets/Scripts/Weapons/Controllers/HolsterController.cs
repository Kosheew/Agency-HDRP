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
    
    private readonly List<Weapon> _activeWeapons = new List<Weapon>();
    
    public void EquipNewWeapon(Weapon newWeapon)
    {
        _isSameTypeSwap = _currentWeapon != null && _currentWeapon.AnimType == newWeapon.AnimType;
        
        _previousWeapon = _currentWeapon;
        if (_previousWeapon == null) _previousWeapon = newWeapon;
        _currentWeapon = newWeapon;
        
        if (!_activeWeapons.Contains(newWeapon))
        {
            _activeWeapons.Add(newWeapon);
        }
    }

    public void EquipWeapon()
    {
        if (_currentWeapon == null) return;
        
        _currentWeapon.gameObject.SetActive(true);
        _currentWeapon.transform.SetParent(handTransform);
        
        _currentWeapon.SetActiveLaser(true);
        
        _currentWeapon.transform.localPosition = new Vector3(0.147f, -0.04f, 0.034f);
        _currentWeapon.transform.localRotation = Quaternion.Euler(-2.2f, 98.472f, 88.77f);
    }

    public void UnEquipWeapon()
    {
        if (_previousWeapon == null) return;
        
        Transform holsterTransform = _previousWeapon.AnimType == WeaponAnimType.Pistol ? holsterPistolTransform : holsterRifleTransform;
        Vector3 positionOffset;
        Quaternion rotationOffset;

        if (_previousWeapon.AnimType == WeaponAnimType.Rifle)
        {
            positionOffset = new Vector3(-0.15f, 0.18f, 0.078f);
            rotationOffset = Quaternion.Euler(-8.1f, 127.879f, 89.256f);
        }
        else // Якщо це гвинтівка
        {
            positionOffset = new Vector3(-0.06f, 0.04f, 0.1f);  // Унікальні значення
            rotationOffset = Quaternion.Euler(0, -88f, 0f); // Унікальні значення
        }

        _previousWeapon.SetActiveLaser(false);
        _previousWeapon.transform.SetParent(holsterTransform);
        _previousWeapon.transform.localPosition = positionOffset;
        _previousWeapon.transform.localRotation = rotationOffset;
        
        if (_isSameTypeSwap)
        {
            foreach (var weapon in _activeWeapons)
            {
                if (weapon != _currentWeapon && weapon.AnimType == _previousWeapon.AnimType)
                {
                    weapon.SetActiveLaser(false);
                    weapon.gameObject.SetActive(false);
                }
            }
        }
    }
    
    public void UnEquipWeaponBase()
    {
        if (_previousWeapon == null) return;
        
        Transform holsterTransform = _currentWeapon.AnimType == WeaponAnimType.Pistol ? holsterPistolTransform : holsterRifleTransform;
        Vector3 positionOffset;
        Quaternion rotationOffset;

        if (_currentWeapon.AnimType == WeaponAnimType.Rifle)
        {
            positionOffset = new Vector3(-0.15f, 0.18f, 0.078f);
            rotationOffset = Quaternion.Euler(-8.1f, 127.879f, 89.256f);
        }
        else // Якщо це гвинтівка
        {
            positionOffset = new Vector3(-0.06f, 0.04f, 0.1f);  // Унікальні значення
            rotationOffset = Quaternion.Euler(0, -88f, 0f); // Унікальні значення
        }

        _currentWeapon.transform.SetParent(holsterTransform);
        _currentWeapon.transform.localPosition = positionOffset;
        _currentWeapon.transform.localRotation = rotationOffset;
        
        if (_isSameTypeSwap)
        {
            foreach (var weapon in _activeWeapons)
            {
                if (weapon != _currentWeapon && weapon.AnimType == _previousWeapon.AnimType)
                {
                    weapon.gameObject.SetActive(false);
                }
            }
        }
    }
}
