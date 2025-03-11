using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class DamageCalculatorSimple : IDamageCalculator
{
    private readonly Weapon _weapon;

    public DamageCalculatorSimple(Weapon weapon)
    {
        _weapon = weapon;
    }

    public float CalculateDamage()
    {
        float spreadMultiplier = _weapon.SpreadMultiplier;

        float damageMultiplier = Mathf.Clamp01(1f - (spreadMultiplier * 0.185f));
        
        return _weapon.DamageValue * damageMultiplier;
    }
}
