using System;
using Characters;
using Characters.Enemy;
using Characters.Player;
using Commands;
using InputActions;
using Scene_Manager;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Character Settings")]
    [SerializeField] private CharacterAudioSettings characterAudioSettings;
    
    [Header("Player Settings")]
    [SerializeField] private PlayerController player;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private UserInput userInput;
    
    [Header("Enemy Settings")] 
    [SerializeField] private EnemyController[] enemies;
    [SerializeField] private BattleController battleController;
    
    [Header("Trigger Zone Settings")]
    [SerializeField] private TriggerDetector[] triggerDetectors;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioManager audioManager;
    
    [Header("Quest Swttings")]
    [SerializeField] private QuestManager questManager;
    [SerializeField] private QuestView questView;
    
    [Header("Quest Completes")]
    [SerializeField] private QuestCompleter[] questCompleter;
    
    [Header("Quest Adders")]
    [SerializeField] private QuestAdder[] questAdder;
    
    [Header("Other Views")]
    [SerializeField] private PauseView pauseView;
    
    private CommandInvoker _commandInvoker;
    
    private DependencyContainer _container;
    
    private StateEnemyManager _stateEnemyManager;
    private StatePlayerManager _statePlayerManager;
    
    private StateEnemyFactory _stateEnemyFactory;
    private StatePlayerFactory _statePlayerFactory;

    private CommandPlayerFactory _commandPlayerFactory;
    private CommandEnemyFactory _commandEnemyFactory;
    
    private SceneController _sceneController;
    
    private BinarySaveSystem _saveSystem;
        
    private void Awake()
    {
        Time.timeScale = 1f;
        
        _saveSystem = new BinarySaveSystem();
        
        _container = new DependencyContainer();
        _commandInvoker = new CommandInvoker();
        
        _stateEnemyManager = new StateEnemyManager();
        _statePlayerManager = new StatePlayerManager();
        
        _stateEnemyFactory = new StateEnemyFactory();
        _statePlayerFactory = new StatePlayerFactory();
        
        _commandEnemyFactory = new CommandEnemyFactory();
        _commandPlayerFactory = new CommandPlayerFactory();
        
        RegisterDependency();
            
        Injection();
        
        OnEvent();
    }

    private void RegisterDependency()
    {
        _container.Register(_saveSystem);
        
        _container.Register(_commandInvoker);
        
        _container.Register(audioManager);
        
        _container.Register(characterAudioSettings);
        
        _container.Register(_stateEnemyManager);
        _container.Register(_statePlayerManager);
        
        _container.Register(_stateEnemyFactory);
        _container.Register(_statePlayerFactory);
            
        _container.Register(questManager);
        _container.Register(questView);
        
        _container.Register(_commandEnemyFactory);
        _container.Register(_commandPlayerFactory);
        _container.Register<IPlayer>(player);
        _container.Register<PlayerController>(player);
        _container.Register(userInput);
        _container.Register(_sceneController);
    }

    private void Injection()
    {
        _commandPlayerFactory.Inject(_container);
        _commandEnemyFactory.Inject(_container);
        
        questManager.Inject(_container);
        questView.Inject(_container);
        player.Inject(_container);
        weaponController.Inject(_container);
        
        battleController.Inject(_container);
        
        foreach (var enemy in enemies)
        {
            enemy.Inject(_container);
        }

        foreach (var adder in questAdder)
        {   
            adder.Inject(_container);
        }

        foreach (var completer in questCompleter)
        {
            completer.Inject(_container);
        }

        foreach (var triggerDetector in triggerDetectors)
        {
            triggerDetector.Inject(_container);
        }
    }
    
    private void Update()
    {
        _statePlayerManager.UpdateState(player);
        foreach (var enemy in enemies)
        {
            _stateEnemyManager?.UpdateState(enemy);
        }
    }

    private void LateUpdate()
    {
        foreach (var triggerDetector in triggerDetectors)
        {
            if(triggerDetector.PlayerInside)
                triggerDetector.LateUpdateState();
        }
    }
    
    private void OnEvent()
    {
        userInput.OnPaused += pauseView.Pause;
    }

    private void OnApplicationQuit()
    {
        questManager.SaveQuestProgress();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        questManager.SaveQuestProgress();
    }
}