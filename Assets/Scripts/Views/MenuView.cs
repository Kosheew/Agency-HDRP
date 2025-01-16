using UnityEngine;
using UnityEngine.UI;
using Loader;
using Scene_Manager;

public class MenuView : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    
    [SerializeField] private GameObject mainCanvas;

    private MenuPresenter _menuPresenter;

    public void Inject(DependencyContainer container)
    {
        var sceneController = container.Resolve<SceneController>();
        var sceneLoader = container.Resolve<SceneLoader>();
        _menuPresenter = new MenuPresenter(sceneLoader, sceneController);

        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(_menuPresenter.QuitGame);
    }

    private void OnStartButtonClicked()
    {
        mainCanvas.SetActive(false);
        _menuPresenter.StartGame();
    }
}