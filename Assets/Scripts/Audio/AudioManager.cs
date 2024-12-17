using UnityEngine;

namespace Characters
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource; 

        public void Init()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}
