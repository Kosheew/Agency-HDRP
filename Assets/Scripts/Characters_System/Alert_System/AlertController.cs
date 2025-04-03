using Alert_System;
using Commands;
using UnityEngine;

public class AlertController : MonoBehaviour
{
    private AlertModel _alertModel;

    private CommandEnemyFactory _commandEnemyFactory;
    
    public void Inject(DependencyContainer container)
    {
        _commandEnemyFactory = container.Resolve<CommandEnemyFactory>();
        _alertModel = container.Resolve<AlertModel>();
    }

    public void UpdateState()
    {
        _alertModel.DecreaseAlert(Time.deltaTime);
    }
    
    public void IncreaseAlert(float alertAmount)
    {
        _alertModel.IncreaseAlert(alertAmount);
    }

    public void ResetAlert()
    {
        _alertModel.ResetAlertLevel();
    }
}
