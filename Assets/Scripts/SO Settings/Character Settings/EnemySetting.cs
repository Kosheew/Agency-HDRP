using UnityEngine;
using UnityEngine.Serialization;

namespace CharacterSettings
{
    [CreateAssetMenu(fileName = "Character Settings", menuName = "Character Settings/Create Enemy Settings")]
    public class EnemySetting: CharacterSetting
    {
        [Header("State Settings")]
        [SerializeField] private float attackDistance;
        [SerializeField] private float attackCooldown;
        
        [Header("View Settings")]
        [SerializeField] private float visionDistance = 10f;
        [SerializeField] private float fieldOfViewAngle = 120f; 
        [SerializeField] private LayerMask visionMask; 
        [SerializeField] private float loseTargetDelay = 2f; 
        [SerializeField] private float checkInterval = 0.2f;
        [SerializeField] private float stepRayAngle = 10f;
        public float AttackDistance => attackDistance;
        public float AttackCooldown => attackCooldown;
        public float VisionDistance => visionDistance;
        public float FieldOfViewAngle => fieldOfViewAngle;
        public LayerMask VisionMask => visionMask;
        public float LoseTargetDelay => loseTargetDelay;
        public float CheckInterval => checkInterval;
        public float StepRayAngle => stepRayAngle;
    }
}