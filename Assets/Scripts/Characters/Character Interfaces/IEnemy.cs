using CharacterSettings;
using Commands;
using UnityEngine;
using System.Collections;
using Characters.Character_Interfaces;

namespace Characters.Enemy
{
    public interface IEnemy: IPatrolled, IFootstepCharacterAudio, ICharacterAnimate, ITargetHandler, IAttackCharacterAudio
    {
        public EnemySetting EnemySetting{ get;}
        public Transform MainPosition { get; }
        public CommandEnemyFactory CommandEnemy { get; }
        public Transform EyesPosition { get; }
        
    }
}