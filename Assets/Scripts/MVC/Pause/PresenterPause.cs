using JetBrains.Annotations;
using Scene_Manager;
using UnityEngine;

namespace Paused
{
    public class PresenterPause
    {
        private readonly PauseModel _pauseModel;
        private readonly SceneController _sceneController;
        private readonly PauseView _pauseView;
        
        public PresenterPause(SceneController sceneController, PauseModel pauseModel, PauseView pauseView)
        {
            _sceneController = sceneController;
            _pauseView = pauseView;
            _pauseModel = pauseModel;
            
            _pauseModel.OnPauseStateChanged += OnPauseStateChanged;

            _pauseView.SetPause(false);
        }

        public void OnRestartButtonPressed()
        {
            _sceneController.RestartScene();
            _pauseModel.SetPaused();
        }

        public void OnContinueButtonPressed()
        {
            _pauseModel.SetPaused();
        }

        public void OnMainMenuButtonPressed()
        {
            _sceneController.LoadScene(0);
            _pauseModel.SetPaused();
        }
        
        private void OnPauseStateChanged(bool isPaused)
        {
            _pauseView.SetPause(isPaused);
        }
    }
}