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

    protected bool _isWalking = false;
    protected bool _isCrouching = false;
    protected bool _canShoot = true;

    protected abstract IEnumerator Shoot();
    protected virtual void CanShoot()
    {
        _canShoot = true;
    }
}
