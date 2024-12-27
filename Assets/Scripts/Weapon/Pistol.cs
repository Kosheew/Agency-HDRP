using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Pistol : Weapon
{
    private bool _canReload = true;

    protected override void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _canShoot = false;
        Invoke("CanShoot", _params.TakingTime);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && _canShoot)
            StartCoroutine(Shoot());

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
        if (_bulletsInMagazine > 0)
        {
            _canShoot = false;
            float spread = GetSpread();

            Vector3 spreadDirection = new Vector3(Random.Range(-spread, spread),
            Random.Range(-spread, spread), 0);

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
        }

        if (_bulletsInMagazine <= 0)
        {
            //Play no bullets sound
            Debug.Log("no piu");
        }

        yield return new WaitForSeconds(_params.IntervalBetweenShots);
        _canShoot = true;
        yield break;
    }

    private void Reload()
    {
        _bulletsInMagazine = 8;
        _canShoot = true;
        _canReload = true;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
