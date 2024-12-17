using Scene_Manager;
using UnityEngine;
using UnityEngine.UI;

namespace Paused
{

    public class PauseView : MonoBehaviour
    {
        [SerializeField] private GameObject panelPause; 
        [SerializeField] private Button restartButton;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button menuButton;
            
        private PresenterPause _presenterPause;

        public void Inject(DependencyContainer container)
        {
            var sceneController = container.Resolve<SceneController>();
            var pauseModel = container.Resolve<PauseModel>();
            _presenterPause = new PresenterPause(sceneController, pauseModel,this);

            restartButton.onClick.AddListener(_presenterPause.OnRestartButtonPressed);
            continueButton.onClick.AddListener(_presenterPause.OnContinueButtonPressed);
            menuButton.onClick.AddListener(_presenterPause.OnMainMenuButtonPressed);
        }

        public void SetPause(bool value)
        {
            panelPause.SetActive(value);
        }
    }
}
