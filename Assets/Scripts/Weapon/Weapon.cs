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

    [SerializeField] protected int _bulletsInMagazine;

    [Header("Effects")]
    [SerializeField] protected ParticleSystem _shootingEffect;
    [SerializeField] protected ParticleSystem _bulletEffect;
    [SerializeField] protected ParticleSystem _enemyEffect;

    protected AudioSource _audioSource;
    protected Animator _animator;

    protected bool _canShoot = true;
    protected float _lastTimeShoot;

    private float _speed = 1f;

    public abstract void Init();
    public abstract void Shoot();
    protected void CanShoot() => _canShoot = true;
    protected float GetSpread()
    {
        float spread = 0f;

        if(_speed < 1.5) spread = _params.WalkingSpread;
        else spread = _params.RunningSpread;

        return spread;
    }
}
