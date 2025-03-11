using Characters;
using UnityEngine;
using UnityEngine.Serialization;
using Loader;
using Scene_Manager;

public class Menu: MonoBehaviour
{
    
    [Header("Audio Manager")]
    [SerializeField] private AudioManager audioManager;
    
    [FormerlySerializedAs("mainView")] [SerializeField] private MenuView menuView;
    [FormerlySerializedAs("loadingView")] [SerializeField] private LoaderView loaderView;
    [FormerlySerializedAs("loadingScene")] [SerializeField] private SceneLoader sceneLoader;
    
    private DependencyContainer _container;
    private SceneController _sceneController;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        _container = new DependencyContainer();
        
        
        RegisterDependency();
            
        Injection();
    }

    private void RegisterDependency()
    {
        _container.Register(sceneLoader);
        _container.Register(_sceneController);
    }

    private void Injection()
    {
        loaderView.Inject(_container);
        menuView.Inject(_container);
    }
}