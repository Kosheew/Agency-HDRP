using UnityEngine;
using CharacterSettings;
using Audio;
using Characters.Character_Interfaces;
using InputActions;

namespace Characters
{
    public interface IPlayer: ITargetHandler
    {
        public PlayerSetting PlayerSetting { get; }
        public CharacterAudioSettings CharacterAudioSettings { get; }
        public CharacterController Controller { get;  }
        public UserInput UserInput { get; }
        public IFootstepAudioHandler FootstepHandler { get; }
        public Transform TransformMain { get; }
        public PlayerAnimation PlayerAnimation { get;  }
        public bool Alive { get; set; }
        public bool Sneaked { get; set; }
        public bool Grounded { get; set; }
        
    }
}