using SO_Settings.Character_Settings;
using UnityEngine;

namespace CharacterSettings
{
    [CreateAssetMenu(fileName = "Character Settings", menuName = "Character Settings/Create Player Settings")]
    public class PlayerSetting: CharacterSetting
    {
        [SerializeField] private float maxClamp;
        [SerializeField] private float accelerationRate;
        [SerializeField] private float sprintSpeed = 5.335f;
        
        [Range(0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime;

        [Space(10)] [SerializeField] private float jumpHeight;

        [SerializeField] private float gravity = 9.81f;

        [SerializeField] private float speedChangeRate = 10f;
        public float MaxClamp => maxClamp;
        public float AccelerationRate => accelerationRate;
        public float SprintSpeed => sprintSpeed;
        public float RotationSmoothTime => rotationSmoothTime;
        public float JumpHeight => jumpHeight;
        public float Gravity => gravity;
        public float SpeedChangeRate => speedChangeRate;
    }
}