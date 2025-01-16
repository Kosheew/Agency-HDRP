using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 0)]
public class WeaponParams : ScriptableObject
{
    [SerializeField] private float _range;
    [SerializeField] private float _damage;

    [SerializeField] private float _intervalBetweenShots;
    [SerializeField] private float _takingTime;
    [SerializeField] private float _noSpreadIncreaseInterval;

    [SerializeField] private float _walkingSpread;
    [SerializeField] private float _crouchingSpread;
    [SerializeField] private float _runningSpread;
    [SerializeField] private float _spreadIncreaseFactor;

    [SerializeField] private int _maxBulletsInMagazine;

    [SerializeField] private bool _isEnemy;

    [Header("Clips")]
    [SerializeField] protected AudioClip _shootingSound;
    [SerializeField] protected AudioClip _noBulletsSound;
    [SerializeField] protected AudioClip _reloadingSound;

    public float Range => _range;
    public float Damage => _damage;
    public float IntervalBetweenShots => _intervalBetweenShots;
    public float TakingTime => _takingTime;
    public float NoSpreadIncreaseInterval => _noSpreadIncreaseInterval;
    public float WalkingSpread => _walkingSpread;
    public float CrouchingSpread => _crouchingSpread;
    public float RunningSpread => _runningSpread;
    public float SpreadIncreaseFactor => _spreadIncreaseFactor;
    public int MaxBulletsInMagazine => _maxBulletsInMagazine;
    public bool IsEnemy => _isEnemy;

    public AudioClip ShootingSound => _shootingSound;
    public AudioClip NoBulletsSound => _noBulletsSound;
    public AudioClip ReloadingSound => _reloadingSound;
}
