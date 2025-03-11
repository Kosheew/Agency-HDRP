using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Character Audio Settings", menuName = "Audio Settings/Character Audio Settings")]
public class CharacterAudioSettings : ScriptableObject 
{ 
    [SerializeField] private float stepIntervalWalk;
    [SerializeField] private float stepIntervalRun;
    [SerializeField] private AudioClip[] footstepWalkClips;
    [SerializeField] private AudioClip[] footstepRunClips;
    [SerializeField] private AudioClip[] footstepJumpClips;
    [SerializeField] private AudioClip[] footstepLandClips;
    
    [SerializeField] private AudioClip attackClip;
    public float StepIntervalWalk => stepIntervalWalk;
    public float StepIntervalRun => stepIntervalRun;
    public AudioClip AttackClip => attackClip;
    
    public AudioClip GetRandomFootstepWalkClip()
    {
        var index = Random.Range(0, footstepWalkClips.Length); 
        return footstepWalkClips[index];                       
    }
    public AudioClip GetRandomFootstepRunClip()
    {
        var index = Random.Range(0, footstepRunClips.Length); 
        return footstepRunClips[index];                       
    }
    public AudioClip GetRandomFootstepJumpClip()
    {
        var index = Random.Range(0, footstepJumpClips.Length); 
        return footstepJumpClips[index];                       
    }
    
    public AudioClip GetRandomFootstepLandClip()
    {
        var index = Random.Range(0, footstepLandClips.Length); 
        return footstepLandClips[index];                       
    }
}

