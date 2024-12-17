using UnityEngine;

[CreateAssetMenu(fileName = "Character Audio Settings", menuName = "Audio Settings/Character Audio Settings")]
public class CharacterAudioSettings : ScriptableObject 
{ 
    [SerializeField] private float stepInterval; 
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip attackClip;
    public float StepInterval => stepInterval;

    public AudioClip AttackClip => attackClip;
    public AudioClip GetRandomFootstepClip()
    {
        var index = Random.Range(0, footstepClips.Length); 
        return footstepClips[index];                       
    }
}

