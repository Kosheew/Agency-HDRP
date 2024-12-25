using Characters;
using Characters.Player;
using CharacterSettings;
using Commands;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Audio;
namespace Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour, IEnemy
    {
        
        [SerializeField] private Transform[] patrolTargets;
        [SerializeField] private EnemySetting enemySetting;
        
        [SerializeField] private Transform eyesPosition;

        [SerializeField] private Weapon weapon;
        
        private AudioSource AudioSource => GetComponent<AudioSource>();
        public NavMeshAgent Agent => GetComponent<NavMeshAgent>();
        private Animator Animator => GetComponent<Animator>();
        public EnemySetting EnemySetting => enemySetting;
        public Transform[] PatrolTargets => patrolTargets;

        public CharacterAnimator CharacterAnimator { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public IPlayer TargetPlayer { get; private set; }
        public AttackAudioHandler AttackAudio { get; private set; }
        public Transform MainPosition => transform;

        private CharacterAudioSettings _characterAudioSettings;
        public CommandEnemyFactory CommandEnemy { get; private set; }
        public Transform EyesPosition => eyesPosition;
        
        public void Inject(DependencyContainer container)
        {
            Agent.speed = enemySetting.MoveSpeed;
            
            _characterAudioSettings = enemySetting.CharacterAudioSettings;
            TargetPlayer = container.Resolve<IPlayer>();
            
            CommandEnemy = container.Resolve<CommandEnemyFactory>();

            FootstepHandler = new FootstepAudioAudioHandler(AudioSource, _characterAudioSettings);
            AttackAudio = new AttackAudioHandler(AudioSource, _characterAudioSettings);
            CharacterAnimator = new CharacterAnimator(Animator);
            
            CommandEnemy.CreatePatrolledCommand(this);
        }

        public Coroutine StartTheCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public void StopTheCoroutine(Coroutine routine)
        {
            StopCoroutine(routine);
        }
        
        //public void ActiveWeapon()
        //{
        //    weapon.ActiveCollider();
        //}

        //public void DeactiveWeapon()
        //{
        //    weapon.DeactivateCollider();
        //}
        
    }
}
