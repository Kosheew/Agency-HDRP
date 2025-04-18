using UnityEngine;
using Commands;
using CharacterSettings;
using Audio;
using Characters.Character_Interfaces;
using Health_System;
using InputActions;
using Views;
using Weapons;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(UserInput))]
    public class PlayerContext : MonoBehaviour, ITargetHandler, IFootstepCharacterAudio
    {
        [SerializeField] private PlayerSetting playerSetting;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private HealthView healthView;
        [SerializeField] private Transform pivot;
        
        public Transform Pivot => pivot;
        public CharacterAudioSettings CharacterAudioSettings { get; private set; }
        public CharacterController Controller { get; private set; }
        public UserInput UserInput { get; private set; }
        public PlayerSetting PlayerSetting => playerSetting;
        public PlayerAnimation PlayerAnimation { get; private set; }
        public Camera MainCamera { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public Transform TransformMain => transform;
        public bool TargetAlive => Alive;
        public Transform TargetPosition { get; set; }
        
        public AlertController AlertController { get; private set; }
        public Weapon Weapon { get; set; }
        
        private CommandPlayerFactory _commandFactory;
        public bool Alive { get; set; }
        public bool Sneaked { get; set; }
        public bool Grounded { get; set; }
        
        
        public void Inject(DependencyContainer container)
        {
            Alive = true;
            _commandFactory = container.Resolve<CommandPlayerFactory>();
            
            Controller = GetComponent<CharacterController>();
            UserInput = GetComponent<UserInput>();
            PlayerAnimation = GetComponent<PlayerAnimation>();
            AlertController = GetComponent<AlertController>();
            
            PlayerAnimation.Init();
            
            CharacterAudioSettings = playerSetting.CharacterAudioSettings;
            
            FootstepHandler = new FootstepAudioAudioHandler(audioSource, CharacterAudioSettings);
            
            GetComponent<HealthComponent>().Init();
            GetComponent<HealthRegenerationComponent>().Init();
            
            healthView.SetHealth(GetComponent<HealthComponent>().MaxHealth);
            MainCamera = Camera.main;
            
            TargetPosition = transform;
            
            _commandFactory.CreateBaseState(this);
            _commandFactory.CreateRegularCommand(this);

        }

        private void LateUpdate()
        {
            UserInput.ResetInput();
        }

        private void OnHit()
        {
            PlayerAnimation.SetHit();
        }

        private void OnDeath()
        {
            _commandFactory.CreateDeadCommand(this);
        }
    }
}
