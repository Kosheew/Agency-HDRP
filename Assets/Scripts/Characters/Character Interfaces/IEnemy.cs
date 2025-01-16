using CharacterSettings;
using Commands;
using UnityEngine;
using Characters.Character_Interfaces;

namespace Characters.Enemy
{
    public interface IEnemy: IPatrolled, IFootstepCharacterAudio, ICharacterAnimate, IAttackCharacterAudio
    {
        public EnemySetting EnemySetting{ get;}
        public Transform MainPosition { get; }
        public CommandEnemyFactory CommandEnemy { get; }
        public VisionChecker VisionChecker { get; }
        public Transform EyesPosition { get; }
        bool ShouldCheckTarget { get; set; } 
        ITargetHandler Target { get; set; }
        
    }
}