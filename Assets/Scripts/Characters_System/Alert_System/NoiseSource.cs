using Characters.Enemy;
using UnityEngine;

public class NoiseSource : MonoBehaviour
{
    public float noiseRadius = 5f;
    public LayerMask aiMask;

    private AlertManager _alertManager;
    
    public void Inject(DependencyContainer container)
    {
        _alertManager = container.Resolve<AlertManager>();
    }
    
    public void MakeNoise() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noiseRadius, aiMask);
        foreach (Collider col in hitColliders)
        {
            var enemyContext = col.GetComponent<AlertController>();
            _alertManager.RegisterEnemy(enemyContext);
            
        }
    }
}
