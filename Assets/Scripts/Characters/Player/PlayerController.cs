using UnityEngine;
using Commands;
using CharacterSettings;
using Audio;
using Characters.Health;
using InputActions;
using Views;
using Weapons;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(UserInput))]
    public class PlayerController : MonoBehaviour, IPlayer
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
        public HealthComponent HealthComponent { get; private set; }
        public Camera MainCamera { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public Transform TransformMain => transform;
        public bool TargetAlive => Alive;
        public Transform TargetPosition => transform;

        public Weapon Weapon { get; set; }
        
        private CommandPlayerFactory _commandFactory;
        public bool Alive { get; set; }
        public bool Sneaked { get; set; }
        public bool Grounded { get; set; }
        
        private RegenerationSystem _regenerationSystem;
        
        public void Inject(DependencyContainer container)
        {
            Alive = true;
            _commandFactory = container.Resolve<CommandPlayerFactory>();
            
            Controller = GetComponent<CharacterController>();
            UserInput = GetComponent<UserInput>();
            PlayerAnimation = GetComponent<PlayerAnimation>();
            
            PlayerAnimation.Init();
            
            CharacterAudioSettings = playerSetting.CharacterAudioSettings;
            
            FootstepHandler = new FootstepAudioAudioHandler(audioSource, CharacterAudioSettings);
            HealthComponent = new HealthComponent(100);
            healthView.SetHealth(100);
            _regenerationSystem = new RegenerationSystem(HealthComponent, 5, 2, this);
            MainCamera = Camera.main;
            
            _commandFactory.CreateBaseState(this);
            _commandFactory.CreateRegularCommand(this);
            HealthComponent.OnHealthChanged += healthView.UpdateHealth;
            HealthComponent.OnTakeDamage += OnHit;
            HealthComponent.OnDeath += OnDeath;
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
