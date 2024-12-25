using System;
using Wallet;
using Characters;
using InitGame.Level;
using Characters.Enemy;
using Characters.Player;
using Commands;
using InputActions;
using Keys;
using Loader;
using Scene_Manager;
using Timer;
using Paused;
using UnityEngine;
using UnityEngine.InputSystem.Users;
using UnityEngine.Serialization;
using UserController;

public class Game : MonoBehaviour
{
    [Header("View Components")]
    [SerializeField] private WalletView walletView;
    [SerializeField] private TimerView timerView;
    [SerializeField] private PauseView pauseView;
    [FormerlySerializedAs("loadingView")] [SerializeField] private LoaderView loaderView;
    
    [Header("Player Settings")]
    [SerializeField] private PlayerController player;
        
    [Header("Game Completion")]
    [SerializeField] private GameCompleted gameCompleted;
    [FormerlySerializedAs("loadingScene")] [SerializeField] private SceneLoader sceneLoader;
    
    [Header("Enemy Manager")] 
    [SerializeField] private EnemyController[] enemies;

    [Header("Key Manager")] 
    [SerializeField] private Key[] keys;

    [Header("Weapons")] 
    [SerializeField] private Weapon[] weapons;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioManager audioManager;
    
    [SerializeField] private CharacterAudioSettings characterAudioSettings;
    
    private CommandInvoker _commandInvoker;
    
    private WalletModel _wallet;
    private TimerModel _timer;
    private PauseModel _pause;
    
    private DependencyContainer _container;
    
    private StateEnemyManager _stateEnemyManager;
    private StatePlayerManager _statePlayerManager;
    
    private StateEnemyFactory _stateEnemyFactory;
    private StatePlayerFactory _statePlayerFactory;

    private CommandPlayerFactory _commandPlayerFactory;
    private CommandEnemyFactory _commandEnemyFactory;
    
    private SceneController _sceneController;
    
    
    
    private IUserInputs _userInputs;
        
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        
        _container = new DependencyContainer();
        
        _wallet = new WalletModel(keys.Length);
        _timer = new TimerModel();
        _pause = new PauseModel();
        
        _userInputs = new UserInput();
        
        _commandInvoker = new CommandInvoker();
        
        _stateEnemyManager = new StateEnemyManager();
        _statePlayerManager = new StatePlayerManager();
        
        _stateEnemyFactory = new StateEnemyFactory();
        _statePlayerFactory = new StatePlayerFactory();
        
        _commandEnemyFactory = new CommandEnemyFactory();
        _commandPlayerFactory = new CommandPlayerFactory();
        
        _sceneController = new SceneController(sceneLoader);
        
        IUserInputs input = gameObject.AddComponent<UserInput>();
        
        RegisterDependency();
            
        Injection();
            
        //Init();
        
        InitView();
    }

    private void RegisterDependency()
    {
        _container.Register(_userInputs);
        
        _container.Register(_commandInvoker);
        
        _container.Register(_wallet);
        _container.Register(audioManager);
        
        _container.Register(characterAudioSettings);
        
        _container.Register(_stateEnemyManager);
        _container.Register(_statePlayerManager);
        
        _container.Register(_stateEnemyFactory);
        _container.Register(_statePlayerFactory);
        
        _container.Register(_commandEnemyFactory);
        _container.Register(_commandPlayerFactory);
        _container.Register<IPlayer>(player);
        
        _container.Register(_sceneController);
        _container.Register(_pause);
        _container.Register(sceneLoader);
    }

    private void Injection()
    {
        _commandPlayerFactory.Inject(_container);
        _commandEnemyFactory.Inject(_container);
        
        player.Inject(_container);

        foreach (var enemy in enemies)
        {
            enemy.Inject(_container);
        }

        foreach (var key in keys)
        {
            key.Inject(_container);
        }
        
        pauseView.Inject(_container);
        loaderView.Inject(_container);
    }
        
    //private void Init()
    //{
    //    audioManager.Init();

    //    foreach (var weapon in weapons)
    //    {
    //        weapon.Init();
    //    }
    //}

    private void InitView()
    {
        new WalletController(_wallet, walletView);
        new TimerController(_timer, timerView);
        
    }


    private void Update()
    {
        _statePlayerManager.UpdateState(player);
        foreach (var enemy in enemies)
        {
            _stateEnemyManager?.UpdateState(enemy);
        }
        
        _timer.UpdateTimer(Time.deltaTime);
    }

    private void LateUpdate()
    {
        /*if (_userInputs.IsPausing() && !player.Alive)
        {
            _pause.SetPaused();
            _timer.StartTimer();
        }*/
    }

    private void OnDestroy()
    {
        _timer.StopTimer();
    }
}