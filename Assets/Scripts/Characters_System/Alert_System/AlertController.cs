using System.Collections;
using System.Collections.Generic;
using Characters.Enemy;
using Commands;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    public float alertLevel = 0f; // 0 – спокій, 1 – максимальна тривога
    public float maxAlertLevel = 1f;
    public float alertDecayRate = 0.1f; // Як швидко AI заспокоюється

    private Vector3 lastHeardNoise;
    private EnemyContext enemyAI;

    private CommandEnemyFactory _commandEnemyFactory;
    
    public void Inject(DependencyContainer container)
    {
        _commandEnemyFactory = container.Resolve<CommandEnemyFactory>();
    }

    public void UpdateAlert() {
        if (alertLevel > 0) {
            alertLevel -= alertDecayRate * Time.deltaTime;
            if (alertLevel <= 0) alertLevel = 0;
        }
    }

    public void IncreaseAlert(Vector3 noisePosition, float alertAmount) {
        lastHeardNoise = noisePosition;
        alertLevel += alertAmount;
        alertLevel = Mathf.Clamp(alertLevel, 0, maxAlertLevel);

        Debug.Log("🔸 Ворог почув шум! Рівень тривоги: " + alertLevel);

        if (alertLevel >= maxAlertLevel) {
            _commandEnemyFactory.CreateAlertCommand(enemyAI);
        }
    }
}
