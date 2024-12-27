using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 0)]
public class WeaponParams : ScriptableObject
{
    [SerializeField] private float _range;
    [SerializeField] private float _damage;

    [SerializeField] private float _intervalBetweenShots;
    [SerializeField] private float _reloadingTime;
    [SerializeField] private float _takingTime;

    [SerializeField] private float _walkingSpread;
    [SerializeField] private float _crouchingSpread;
    [SerializeField] private float _runningSpread;

    [SerializeField] private int _maxBulletsInMagazine;

    public float Range => _range;
    public float Damage => _damage;
    public float IntervalBetweenShots => _intervalBetweenShots;
    public float ReloadingTime => _reloadingTime;
    public float TakingTime => _takingTime;
    public float WalkingSpread => _walkingSpread;
    public float CrouchingSpread => _crouchingSpread;
    public float RunningSpread => _runningSpread;
    public int MaxBulletsInMagazine => _maxBulletsInMagazine;
}
