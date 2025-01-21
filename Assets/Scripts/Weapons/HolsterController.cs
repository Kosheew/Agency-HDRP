using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsterController : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    
    [SerializeField] private Transform holsterTransform;
    [SerializeField] private Transform handTransform;

    private Vector3 _holsterPosition;
    private Vector3 _holsterRotation;

    private void Start()
    {
        _holsterPosition = holsterTransform.localPosition;
        _holsterRotation = holsterTransform.localRotation.eulerAngles;
    }


    public void EquipWeapon()
    {
        weapon.transform.SetParent(handTransform);
        weapon.transform.localPosition = new Vector3(0.147f, -0.04f, 0.034f);
        weapon.transform.localRotation = Quaternion.Euler(-2.2f, 98.472f, 88.77f);
    }

    public void UnEquipWeapon()
    {
        weapon.transform.SetParent(holsterTransform);
        weapon.transform.localPosition = new Vector3(-0.15f, 0.18f, 0.078f);
        weapon.transform.localRotation = Quaternion.Euler(-8.1f, 127.879f, 89.256f);
    }
}
