using SO_Settings.Character_Settings;
using UnityEngine;

namespace CharacterSettings
{
    [CreateAssetMenu(fileName = "Character Settings", menuName = "Character Settings/Create Player Settings")]
    public class PlayerSetting: CharacterSetting
    {
        [SerializeField] private float maxClamp;
        [SerializeField] private float accelerationRate;
        
        public float MaxClamp => maxClamp;
        public float AccelerationRate => accelerationRate;
    }
}