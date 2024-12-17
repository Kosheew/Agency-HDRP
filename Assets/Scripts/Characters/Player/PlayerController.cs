using System;
using UnityEngine;
using UserController;
using Commands;
using CharacterSettings;
using Characters;
using Audio;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterTouch))]
    public class PlayerController : MonoBehaviour, IPlayer
    {
        [SerializeField] private PlayerSetting playerSetting;
        [SerializeField] private AudioSource audioSource;
        public CharacterAudioSettings CharacterAudioSettings { get; private set; }
        public CharacterController Controller { get; private set; }
        public Camera CameraMain { get; private set; }
        public PlayerSetting PlayerSetting => playerSetting;
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public IUserInputs UserInputs { get; private set; }
        public Transform TransformMain => transform;
        
        public bool Alive { get; set; }
        
        private CommandPlayerFactory _commandFactory;
        
        public void Inject(DependencyContainer container)
        {
            _commandFactory = container.Resolve<CommandPlayerFactory>();
            
            Controller = GetComponent<CharacterController>();
            UserInputs = container.Resolve<IUserInputs>();
            CharacterAudioSettings = playerSetting.CharacterAudioSettings;
            
            FootstepHandler = new FootstepAudioAudioHandler(audioSource, CharacterAudioSettings);
            CameraMain = Camera.main;
            
            _commandFactory.CreateMoveCommand(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out Weapon weapon))
            {
                _commandFactory.CreateDeadCommand(this);
            }
        }
    }
}
