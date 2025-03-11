using CharacterSettings;
using Commands;
using UnityEngine;
using Characters.Character_Interfaces;
using Weapons;

namespace Characters.Enemy
{
    public interface IEnemy: IPatrolled, IFootstepCharacterAudio, ICharacterAnimate, IAttackCharacterAudio, IHealthCharacter
    {
        public EnemySetting EnemySetting{ get;}
        public Transform MainPosition { get; }
        public CommandEnemyFactory CommandEnemy { get; }
        public VisionChecker VisionChecker { get; }
        public Transform EyesPosition { get; }
        public Weapon Weapon { get; }
        bool ShouldCheckTarget { get; set; } 
        ITargetHandler Target { get; set; }
        
    }
}