using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    #region NotNow
    //private Collider _collider;

    //public void Init()
    //{
    //    _collider = GetComponent<Collider>();
    //    DeactivateCollider();
    //}

    //public void ActiveCollider()
    //{
    //    _collider.enabled = true;
    //}

    //public void DeactivateCollider()
    //{
    //    _collider.enabled = false;
    //}
    #endregion
    [SerializeField] protected WeaponParams _params;

    [SerializeField] protected Transform _spawnPoint;

    [SerializeField] protected int _bullets;
    [SerializeField] protected int _bulletsInMagazine;

    [Header("Effect")]
    [SerializeField] protected ParticleSystem _shootingEffect;
    [SerializeField] protected ParticleSystem _bulletEffect;
    [SerializeField] protected ParticleSystem _enemyEffect;

    [Header("Clips")]
    [SerializeField] protected AudioClip _shootingSound;
    [SerializeField] protected AudioClip _noBulletsSound;
    [SerializeField] protected AudioClip _reloadingSound;

    protected AudioSource _audioSource;
    protected Animator _animator;

    private bool _isWalking = false;
    private bool _isCrouching = false;
    private bool _isRunning = false;

    protected bool _canShoot = true;

    protected abstract void Init();
    protected abstract IEnumerator Shoot();
    protected void CanShoot() => _canShoot = true;
    protected float GetSpread()
    {
        float spread = 0f;

        if(_isWalking) spread = _params.WalkingSpread;
        if(_isCrouching) spread = _params.CrouchingSpread;
        if(_isRunning) spread = _params.RunningSpread;

        return spread;
    }
}
