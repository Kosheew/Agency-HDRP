using UnityEngine;
using UnityEngine.UI;

namespace Alert_System
{
    public class AlertView: MonoBehaviour
    {
        [SerializeField] public Image alertBar;
        
        private AlertModel _alertModel;

        public void Inject(DependencyContainer container)
        {
            _alertModel = container.Resolve<AlertModel>();
            _alertModel.OnAlertChange += UpdateAlertBar;
        }

        private void UpdateAlertBar(float newAlertLevel)
        {
            alertBar.fillAmount = newAlertLevel / _alertModel.maxAlertLevel;
        }
    }
}