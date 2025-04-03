using Characters;
using Characters.Player;
using CharacterSettings;
using Commands;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Audio;
using Characters.Character_Interfaces;
using Characters.Health;
using Weapons;

namespace Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Animator))]
    public class EnemyContext : MonoBehaviour, IPatrolled, IFootstepCharacterAudio, ICharacterAnimate, IAttackCharacterAudio, IHealthCharacter
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

        public HealthComponent HealthComponent { get; private set; }
        public CharacterAnimator CharacterAnimator { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public AttackAudioHandler AttackAudio { get; private set; }
        public Transform MainPosition => transform;

        private CharacterAudioSettings _characterAudioSettings;
        public CommandEnemyFactory CommandEnemy { get; private set; }
        public VisionChecker VisionChecker { get; private set; }
        public Weapon Weapon => weapon;
        public Transform EyesPosition => eyesPosition;
        
        private Collider _collider;
        
        public bool ShouldCheckTarget
        {
            get => checkTarget;
            set => checkTarget = value;
        }
        
        public ITargetHandler Target { get; set; }
        
        public Transform TargetTransform { get; set; }
        
        [SerializeField] private bool checkTarget;

        [SerializeField] private bool patrolled;
        
        
        public void Inject(DependencyContainer container)
        {
            Agent.speed = enemySetting.MoveSpeed;
            
            _characterAudioSettings = enemySetting.CharacterAudioSettings;
            
            CommandEnemy = container.Resolve<CommandEnemyFactory>();
            
            FootstepHandler = new FootstepAudioAudioHandler(AudioSource, _characterAudioSettings);
            AttackAudio = new AttackAudioHandler(AudioSource, _characterAudioSettings);
            CharacterAnimator = new CharacterAnimator(Animator);
            VisionChecker = new VisionChecker(enemySetting.LoseTargetDelay);
            HealthComponent = new HealthComponent(50);
            _collider = GetComponent<Collider>();
            weapon.Init();
            
            if(patrolled)
                CommandEnemy.CreatePatrolledCommand(this);
            else
                CommandEnemy.CharacterIdleCommand(this);
            
            HealthComponent.OnDeath += OnDeath;
            HealthComponent.OnHealthChanged += OnDamageable;
        }
        
        private void OnDamageable(float damage)
        {
            ShouldCheckTarget = true;
            CommandEnemy.CreateToAttackedCommand(this);
        }
        private void OnDeath()
        {
            CommandEnemy.CreateDeathCommand(this);
            _collider.enabled = false;
        }
    }
}
