using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Collider _collider;

    public void Init()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void ActiveCollider()
    {
        _collider.enabled = true;
    }

    public void DeactiveCollider()
    {
        _collider.enabled = false;
    }
    
}
