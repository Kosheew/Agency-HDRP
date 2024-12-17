using UnityEngine;
using UnityEngine.SceneManagement;
using Loader;

namespace Scene_Manager
{
    public class SceneController
    {
        private SceneLoader _sceneLoader;
        public SceneController(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }
        
        public void RestartScene()
        {
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            _sceneLoader.LoadSceneAsync(currentIndex);
        }

        public void LoadScene(int index)
        {
            _sceneLoader.LoadSceneAsync(index);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}