using UnityEngine;
using CharacterSettings;
using Audio;
using InputActions;

namespace Characters
{
    public interface IPlayer
    {
        public PlayerSetting PlayerSetting { get; }
        public CharacterAudioSettings CharacterAudioSettings { get; }
        public CharacterController Controller { get;  }
        public UserInput UserInput { get; }
        public IFootstepAudioHandler FootstepHandler { get; }
        public Transform TransformMain { get; }
        public PlayerAnimation PlayerAnimation { get;  }
        public bool Alive { get; set; }
        
    }
}