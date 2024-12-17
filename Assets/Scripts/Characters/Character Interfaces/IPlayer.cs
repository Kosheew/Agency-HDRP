using UnityEngine;
using UserController;
using CharacterSettings;
using Characters;
using Audio;
namespace Characters
{
    public interface IPlayer
    {
        public PlayerSetting PlayerSetting { get; }
        public CharacterAudioSettings CharacterAudioSettings { get; }
        public CharacterController Controller { get;  }
        public Camera CameraMain { get; }
        public IUserInputs UserInputs { get; }
        public IFootstepAudioHandler FootstepHandler { get; }
        public Transform TransformMain { get; }
        public bool Alive { get; set; }
    }
}