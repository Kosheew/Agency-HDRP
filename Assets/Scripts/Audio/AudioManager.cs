using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; } 

    private AudioSource _audioSource;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;

        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;
        
        DontDestroyOnLoad(gameObject);
    }
    
    public void PlayDialogueAudio(AudioClip clip)
    {
        if(clip == null) return;

        StopAudio();
        
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    public void StopAudio()
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}