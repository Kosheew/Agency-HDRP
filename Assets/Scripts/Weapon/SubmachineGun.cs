using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class SubmachineGun : Weapon
{
    public override void Init()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _canShoot = false;
        Invoke("CanShoot", _params.TakingTime);
    }

    public override void Shoot()
    {
        
    }

    public IEnumerator Shootick()
    {
        if (_bulletsInMagazine > 0 || _params.IsEnemy) 
        {
            float spread = GetSpread();

            if (Time.time - _lastTimeShoot < _params.NoSpreadIncreaseInterval)
                spread *= _params.SpreadIncreaseFactor;

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
            _lastTimeShoot = Time.time;

            yield return new WaitForSeconds(_params.IntervalBetweenShots);
            StartCoroutine(Shootick());
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
