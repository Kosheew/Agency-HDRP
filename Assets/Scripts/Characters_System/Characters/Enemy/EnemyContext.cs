using CharacterSettings;
using Commands;
using UnityEngine;
using UnityEngine.AI;
using Audio;
using Characters.Character_Interfaces;
using CustomAI.Handlers;
using Health_System;
using Weapons;

namespace Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent), typeof(AudioSource), typeof(Animator))]
    public class EnemyContext : MonoBehaviour, IFootstepCharacterAudio, ICharacterAnimate, IAttackCharacterAudio
    {
        [SerializeField] private EnemySetting enemySetting;
        public EnemySetting EnemySetting => enemySetting;
        
        [SerializeField] private Transform eyesPosition;
        public Transform EyesPosition => eyesPosition;
        
        [SerializeField] private Weapon weapon;
        public Weapon Weapon => weapon;
        
        [SerializeField] private bool checkTarget;

        [SerializeField] private bool patrolled;
        
        private AudioSource AudioSource;
        public NavMeshAgent Agent {get; set;}
        private Animator Animator;
        public AIHandlerComponent AIHandler {get; set;}
        public RotationHandler RotationHandler {get; set;}

        public HealthComponent HealthComponent { get; private set; }
        public CharacterAnimator CharacterAnimator { get; private set; }
        public IFootstepAudioHandler FootstepHandler { get; private set; }
        public AttackAudioHandler AttackAudio { get; private set; }
        
        public Transform MainPosition => transform;

        private CharacterAudioSettings _characterAudioSettings;
        public CommandEnemyFactory CommandEnemy { get; private set; }
        public VisionChecker VisionChecker { get; private set; }
        
        private Collider _collider;
        
        public bool ShouldCheckTarget
        {
            get => checkTarget;
            set => checkTarget = value;
        }
        
        public Transform TargetTransform { get; set; }
        
        
        public void Inject(DependencyContainer container)
        {
            CommandEnemy = container.Resolve<CommandEnemyFactory>();
            
            GetComponentsEnemy();
            SetSettings();
            CreateComponents();
            InitState();
            Subscribers();
        }

        private void CreateComponents()
        {
            FootstepHandler = new FootstepAudioAudioHandler(AudioSource, _characterAudioSettings);
            AttackAudio = new AttackAudioHandler(AudioSource, _characterAudioSettings);
            CharacterAnimator = new CharacterAnimator(Animator);
            VisionChecker = new VisionChecker(enemySetting.LoseTargetDelay);
           // HealthComponent = new HealthComponent(enemySetting.Health);
        }
        
        private void GetComponentsEnemy()
        {
            _collider = GetComponent<Collider>();
            AudioSource = GetComponent<AudioSource>();
            Agent = GetComponent<NavMeshAgent>();
            Animator = GetComponent<Animator>();
            AIHandler = GetComponent<AIHandlerComponent>();
            RotationHandler = GetComponent<RotationHandler>();
        }

        private void SetSettings()
        {
            Agent.speed = enemySetting.MoveSpeed;
            _characterAudioSettings = enemySetting.CharacterAudioSettings;
        }

        private void InitState()
        {
            AIHandler.Init(enemySetting);
            weapon.Init();
            
            if(patrolled)
                CommandEnemy.CreatePatrolledCommand(this);
            else
                CommandEnemy.CharacterIdleCommand(this);
        }
        
        private void Subscribers()
        {
          //  HealthComponent.OnDeath += OnDeath;
          //  HealthComponent.OnHealthChanged += OnDamageable;
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
