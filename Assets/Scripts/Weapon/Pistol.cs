using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Pistol : Weapon
{
    private bool _canReload = true;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _canShoot = false;
        Invoke("CanShootick", _params.TakingTime);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartCoroutine(Shoot());

        if (Input.GetMouseButtonUp(0))
            StopAllCoroutines();

        if (Input.GetKeyDown(KeyCode.R) && _canReload &&  _bulletsInMagazine < _params.MaxBulletsInMagazine)
        {
            Invoke("Reload", _params.ReloadingTime);
            _canShoot = false;
            _canReload = false;
            //play reloading animation
            //play reloading sound
        }
    }

    protected override IEnumerator Shoot()
    {
        if (_bulletsInMagazine > 0 && _canShoot)
        {
            RaycastHit hit;
            if (Physics.Raycast(_spawnPoint.position, _spawnPoint.forward, out hit, _params.Range))
            {
                //Take damage
                //Play shooting sound
                //Play shooting effect
                _bulletsInMagazine--;
                yield break;
            }
        }

        if (_bulletsInMagazine <= 0)
        {
            //Play no bullets sound
            yield break;
        }
    }

    private void Reload()
    {
        int bulletsToAdd = _params.MaxBulletsInMagazine - _bullets;

        _bulletsInMagazine += bulletsToAdd;

        if (_bulletsInMagazine > _params.MaxBulletsInMagazine)
            _bulletsInMagazine = _params.MaxBulletsInMagazine;

        _canShoot = true;
        _canReload = true;

        if (_bullets < 0)
            _bullets = 0;
    }

    private void CanShootick()
    {
        base.CanShoot();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
