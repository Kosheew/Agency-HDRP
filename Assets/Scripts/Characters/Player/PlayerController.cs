using UnityEngine;
using Commands;
using CharacterSettings;
using Audio;
using InputActions;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterTouch))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(UserInput))]
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [SerializeField] private PlayerSetting playerSetting;
        [SerializeField] private AudioSource audioSource;
        public CharacterAudioSettings CharacterAudioSettings { get; private set; }
        public CharacterController Controller { get; private set; }
        public UserInput UserInput { get; private set; }
        public PlayerSetting PlayerSetting => playerSetting;
        public PlayerAnimation PlayerAnimation { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public Transform TransformMain => transform;
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
            
            PlayerAnimation.Init();
            
            CharacterAudioSettings = playerSetting.CharacterAudioSettings;
            
            FootstepHandler = new FootstepAudioAudioHandler(audioSource, CharacterAudioSettings);
            
            _commandFactory.CreateRegularCommand(this);
        }

        private void LateUpdate()
        {
            UserInput.ResetInput();
        }
    }
}
