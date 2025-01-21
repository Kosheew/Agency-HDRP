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
        

        public void PlayFootstepWalkSound()
        {
            if (!(Time.time >= _nextStep)) return;
            var clip = CharacterAudioSettings.GetRandomFootstepWalkClip();
            AudioSource.PlayOneShot(clip);        
            _nextStep = Time.time + CharacterAudioSettings.StepIntervalWalk;   
        }

        public void PlayFootstepRunSound()
        {
            if (!(Time.time >= _nextStep)) return;
            var clip = CharacterAudioSettings.GetRandomFootstepRunClip();
            AudioSource.PlayOneShot(clip);        
            _nextStep = Time.time + CharacterAudioSettings.StepIntervalRun;   
        }

        public void PlayFootstepJumpSound()
        {
            if (!(Time.time >= _nextStep)) return;
            var clip = CharacterAudioSettings.GetRandomFootstepJumpClip();
            AudioSource.PlayOneShot(clip);        
            _nextStep = Time.time + CharacterAudioSettings.StepIntervalWalk;   
        }

        public void PlayFootstepLandSound()
        {
            if (!(Time.time >= _nextStep)) return;
            var clip = CharacterAudioSettings.GetRandomFootstepLandClip();
            AudioSource.PlayOneShot(clip);        
            _nextStep = Time.time + CharacterAudioSettings.StepIntervalWalk;   
        }
    }
}
