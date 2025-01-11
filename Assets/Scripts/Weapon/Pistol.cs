using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Pistol : Weapon
{
    [SerializeField] protected int _bullets;
    [SerializeField] private float _reloadingTime;

    private bool _canReload = true;

    public override void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _canShoot = false;
        Invoke(nameof(CanShoot), _params.TakingTime);
    }

    public override void Shoot()
    {
        if ((_bulletsInMagazine > 0 && _canShoot) || _params.IsEnemy)
        {
            _canShoot = false;

            float spread = GetSpread();

            if (Time.time - _lastTimeShoot < _params.NoSpreadIncreaseInterval)
                spread *= _params.SpreadIncreaseFactor;

            Vector3 spreadDirection = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
            Vector3 shootDirection = (_spawnPoint.forward + _spawnPoint.TransformDirection(spreadDirection)).normalized;

            RaycastHit hit;
            if (Physics.Raycast(_spawnPoint.position, shootDirection, out hit, _params.Range))
            {
                //Take damage
                //Play shooting sound
                //Play shooting effect
            }

            _bulletsInMagazine--;
            Debug.Log("piu");

            _lastTimeShoot = Time.time;
            Invoke(nameof(CanShoot), _params.IntervalBetweenShots);
        }
        else if (_bulletsInMagazine <= 0)
        {
            Debug.Log("no piu");
            //Play no bullets sound
        }
    }

    public void StartReloading()
    {
        _canReload = false;
        _canShoot = false;
        Invoke(nameof(Reload), _reloadingTime);
    }

    private void Reload()
    {
        _bulletsInMagazine = _params.MaxBulletsInMagazine;
        _canShoot = true;
        _canReload = true;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
