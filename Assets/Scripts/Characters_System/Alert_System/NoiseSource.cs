using Characters.Enemy;
using Commands;
using UnityEngine;

public class NoiseSource : MonoBehaviour
{
    [SerializeField] private float noiseLevel = 0.5f;
    
    [SerializeField] private float noiseRadius = 5f;
    [SerializeField] private LayerMask aiMask;

    private AlertController _alertController;
    private CommandEnemyFactory _commandFactory;
    
    public void Inject(DependencyContainer container)
    {
        _alertController = container.Resolve<AlertController>();
        _commandFactory = container.Resolve<CommandEnemyFactory>();
    }
    
    public void MakeNoise() 
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, noiseRadius, aiMask);
        
        foreach (Collider col in hitColliders)
        {
            if (col.TryGetComponent(out EnemyContext enemy))
            {
                enemy.TargetTransform = transform;
                _commandFactory.CreateSuspiciousCommand(enemy);
            }
            
            _alertController.IncreaseAlert(noiseLevel);
            
        }
    }
}
