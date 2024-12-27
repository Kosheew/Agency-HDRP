using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Shotgun : Weapon
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
    }

    protected override IEnumerator Shoot()
    {
        _canShoot = false;
        if (_bulletsInMagazine > 0)
        {
            for (int i = 0; i <= 8; i++)
            {
                float spread = GetSpread();
                float baseSpread = 1f + spread;

                Vector3 spreadDirection = new Vector3(Random.Range(-baseSpread, baseSpread),
                Random.Range(-baseSpread, baseSpread), 0);

                Vector3 shootDirection = (_spawnPoint.forward + _spawnPoint.TransformDirection(spreadDirection)).normalized;

                RaycastHit hit;
                if (Physics.Raycast(_spawnPoint.position, shootDirection, out hit, _params.Range))
                {
                    //Take damage
                    //Play shooting sound
                    //Play shooting effect
                }
            }
            _bulletsInMagazine--;
            Debug.Log("Piu");
        }

        if (_bulletsInMagazine <= 0)
        {
            //Play no bullets sound
        }

        yield return new WaitForSeconds(_params.IntervalBetweenShots);
        _canShoot = true;
        yield break;
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
