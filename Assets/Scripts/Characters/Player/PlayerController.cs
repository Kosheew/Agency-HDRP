using UnityEngine;
using Commands;
using CharacterSettings;
using Audio;
using Characters.Enemy;
using Characters.Health;
using InputActions;
using Views;
using Weapons;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterTouch))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(UserInput))]
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [SerializeField] private PlayerSetting playerSetting;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private HealthView healthView;
        
        public CharacterAudioSettings CharacterAudioSettings { get; private set; }
        public CharacterController Controller { get; private set; }
        public UserInput UserInput { get; private set; }
        public PlayerSetting PlayerSetting => playerSetting;
        public PlayerAnimation PlayerAnimation { get; private set; }
        public CharacterHealth CharacterHealth { get; private set; }
        public Camera MainCamera { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public Transform TransformMain => transform;
        public bool TargetAlive => Alive;
        public Transform TargetPosition => transform;

        public Weapon Weapon => _weapon;
        
        private CommandPlayerFactory _commandFactory;
        public bool Alive { get; set; }
        public bool Sneaked { get; set; }
        public bool Grounded { get; set; }
        
        [SerializeField] private Weapon _weapon;
        
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
            CharacterHealth = new CharacterHealth(100);
            healthView.SetHealth(100);
            
            MainCamera = Camera.main;
            
            _weapon.Init();
            
            _commandFactory.CreateRegularCommand(this);
            CharacterHealth.OnHealthChanged += healthView.UpdateHealth;
            CharacterHealth.OnDeath += OnDeath;
        }

        private void LateUpdate()
        {
            UserInput.ResetInput();
        }

        private void OnDeath()
        {
            _commandFactory.CreateDeadCommand(this);
        }
    }
}
