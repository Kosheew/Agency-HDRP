using UnityEngine;

namespace SO_Settings.Character_Settings
{
    public abstract class CharacterSetting: ScriptableObject
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float turnSpeed;
        
        [SerializeField] private CharacterAudioSettings characterAudioSettings;
        
        public float MoveSpeed => moveSpeed;
        public float TurnSpeed => turnSpeed;

        public CharacterAudioSettings CharacterAudioSettings => characterAudioSettings;
        
      
    }
}