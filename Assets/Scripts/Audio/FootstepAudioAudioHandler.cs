using UnityEngine;
using Characters;

namespace Audio
{
    public class FootstepAudioAudioHandler : BaseCharacterAudio, IFootstepAudioHandler
    {
        private float _nextStep= 0;
        
        public FootstepAudioAudioHandler(AudioSource audioSource, CharacterAudioSettings characterAudioSettings) : base(audioSource, characterAudioSettings)
        {
        }
        

        public void PlayFootstepSound()
        {
            if (!(Time.time >= _nextStep)) return;
            var clip = CharacterAudioSettings.GetRandomFootstepClip();
            AudioSource.PlayOneShot(clip);        
            _nextStep = Time.time + CharacterAudioSettings.StepInterval;   
        }
    }
}
