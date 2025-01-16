using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public class Shotgun : Weapon
{
    [SerializeField] private int _cartageAmount = 8;
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
        if (_bulletsInMagazine > 0 || _params.IsEnemy)
        {
            _canShoot = false;

            for (int i = 0; i <= _cartageAmount; i++)
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

        Invoke(nameof(CanShoot), _params.IntervalBetweenShots);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}
