using UnityEngine;

namespace Audio
{
    public class AttackAudioHandler: BaseCharacterAudio, IAttackAudioHandler
    {
        public AttackAudioHandler(AudioSource audioSource, CharacterAudioSettings characterAudioSettings) : base(audioSource, characterAudioSettings)
        {
        }

        public void PlayAttackSound()
        {
            var clip = CharacterAudioSettings.AttackClip;
            AudioSource.PlayOneShot(clip);
        }
    }
}