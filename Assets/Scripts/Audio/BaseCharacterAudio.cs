using UnityEngine;

namespace Audio
{
    public abstract class BaseCharacterAudio
    {
        protected readonly AudioSource AudioSource;
        protected  readonly CharacterAudioSettings CharacterAudioSettings;
        
        protected BaseCharacterAudio(AudioSource audioSource, CharacterAudioSettings characterAudioSettings)
        {
            AudioSource = audioSource;
            CharacterAudioSettings = characterAudioSettings;
        }
    }
}