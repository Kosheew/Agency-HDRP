using Alert_System;
using Zones;
using Characters;
using Characters.Enemy;
using Characters.Player;
using Commands;
using InputActions;
using Scene_Manager;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private GameSaveManager gameSaveManager;
    
    [Header("Character Settings")]
    [SerializeField] private CharacterAudioSettings characterAudioSettings;
    
    [Header("Player Settings")]
    [SerializeField] private PlayerContext player;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private UserInput userInput;
    
    [Header("Enemy Settings")] 
    [SerializeField] private EnemyContext[] enemies;
    [SerializeField] private BattleController battleController;
    
    [Header("Trigger Zone Settings")]
    [SerializeField] private TriggerUseDetector[] triggerDetectors;
    
    [Header("Audio Settings")]
    [SerializeField] private AudioManager audioManager;
    
    [Header("Quest Settings")]
    [SerializeField] private QuestController questController;
    [SerializeField] private QuestView questView;
    
    [Header("Quest Completes")]
    [SerializeField] private QuestCompleter[] questCompleter;
    
    [Header("Quest Adders")]
    [SerializeField] private QuestAdder[] questAdder;
    
    [Header("Other Views")]
    [SerializeField] private PauseView pauseView;
    
    [Header("Evidence System")] 
    [SerializeField] private CluesController cluesController;
    
    [Header("Dialogue System")]
    [SerializeField] private DialogueView dialogueView;
    
    [SerializeField] private DialogueController dialogueController; 
    [SerializeField] private DialogProgressManager dialogProgressManager;
    
    [SerializeField] private NPCDialogueController[] npcDialogueManagers;
    
    [Header("Alert System")]
    [SerializeField] private AlertController alertController;
    [SerializeField] private AlertView alertView;
    
    [Header("Noise Sources")]
    [SerializeField] private NoiseSource[] noiseSources;
    
    private AlertModel _alertModel;
    
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
        
        CreateObject();
        RegisterDependency();
        Injection();
        OnEvent();
    }

    private void CreateObject()
    {
         _saveSystem = new BinarySaveSystem();
                
         _container = new DependencyContainer();
         _commandInvoker = new CommandInvoker();
                
         _stateEnemyManager = new StateEnemyManager();
         _statePlayerManager = new StatePlayerManager();
                
         _stateEnemyFactory = new StateEnemyFactory(enemies);
         _statePlayerFactory = new StatePlayerFactory();
                
         _commandEnemyFactory = new CommandEnemyFactory();
         _commandPlayerFactory = new CommandPlayerFactory();
                
         _alertModel = new AlertModel();
    }

    private void RegisterDependency()
    {
        _container.Register(_saveSystem);
        
        _container.Register(gameSaveManager);
        
        _container.Register(_commandInvoker);
        
        _container.Register(audioManager);
        
        _container.Register(characterAudioSettings);
        
        _container.Register(_stateEnemyManager);
        _container.Register(_statePlayerManager);
        
        _container.Register(_stateEnemyFactory);
        _container.Register(_statePlayerFactory);
            
        _container.Register(questController);
        _container.Register(questView);
        
        _container.Register(_commandEnemyFactory);
        _container.Register(_commandPlayerFactory);
        _container.Register(player);
        _container.Register(userInput);
        _container.Register(_sceneController);
        
        _container.Register(dialogueController);
        _container.Register(cluesController);
        _container.Register(dialogueView);
        _container.Register(dialogProgressManager);
        
        _container.Register(_alertModel);
        _container.Register(alertController);
        _container.Register(alertView);
    }

    private void Injection()
    {
        _commandPlayerFactory.Inject(_container);
        _commandEnemyFactory.Inject(_container);
        
        gameSaveManager.Inject(_container);
        
        alertController.Inject(_container);
        alertView.Inject(_container);
        
        questController.Inject(_container);
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
        
        dialogProgressManager.Inject(_container);
        
        cluesController.Inject(_container);
        dialogueController.Inject(_container);
        dialogueView.Inject(_container);
        
        foreach (var npcDialogueTrigger in npcDialogueManagers)
        {
            npcDialogueTrigger.Inject(_container);
        }
        
        foreach (var noiseSource in noiseSources)
        {
            noiseSource.Inject(_container);
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
            triggerDetector.LateUpdateState();
        }
    }
    
    private void OnEvent()
    {
        userInput.OnPaused += pauseView.Pause;
    }

    private void OnApplicationQuit()
    {
      //  gameSaveManager.SaveQuestProgress(questController);
        
      //  gameSaveManager.SaveNPCProgress(npcDialogueManagers);
        
      //  gameSaveManager.SaveGame();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
      //  gameSaveManager.SaveQuestProgress(questController);
        
       // gameSaveManager.SaveNPCProgress(npcDialogueManagers);
        
      //  gameSaveManager.SaveGame();
    }
}