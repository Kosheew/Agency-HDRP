using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class SubmachineGun : Weapon
{
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

        if (Input.GetMouseButtonUp(0))
        {
            StopAllCoroutines();
            _canShoot = false;
            Invoke("CanShoot", _params.TakingTime);
        }
    }
    protected override IEnumerator Shoot()
    {
        if (_bulletsInMagazine > 0) 
        {
            float spread = GetSpread();
            Vector3 spreadDirection = new Vector3(
                Random.Range(-spread, spread),
                Random.Range(-spread, spread),
                0
            );

            Vector3 shootDirection = (_spawnPoint.forward + _spawnPoint.TransformDirection(spreadDirection)).normalized;

            RaycastHit hit;
            if (Physics.Raycast(_spawnPoint.position, shootDirection, out hit, _params.Range))
            {
                // Play shooting sound
                // Play shooting effect
            }

            _bulletsInMagazine--;
            Debug.Log("piu");

            yield return new WaitForSeconds(_params.IntervalBetweenShots);
            StartCoroutine(Shoot());
        }

        if (_bulletsInMagazine <= 0)
        {
            // Play no bullets sound
            Debug.Log("No piu");
            yield break;
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
