using UnityEngine;

namespace CharacterSettings
{
    [CreateAssetMenu(fileName = "Character Settings", menuName = "Character Settings/Create Player Settings")]
    public class PlayerSetting: CharacterSetting
    {
        [SerializeField] private float accelerationRate;
        
        [Range(0f, 0.3f)]
        [SerializeField] private float rotationSmoothTime;

        [Space(10)] [SerializeField] private float jumpHeight;

        [SerializeField] private float gravity = 9.81f;
        
        public float AccelerationRate => accelerationRate;
        public float RotationSmoothTime => rotationSmoothTime;
        public float JumpHeight => jumpHeight;
        public float Gravity => gravity;
    }
}