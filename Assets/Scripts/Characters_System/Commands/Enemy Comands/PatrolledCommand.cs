using Characters.Enemy;

namespace Characters.Command
{
    public class PatrolledCommand: ICommandEnemy
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        public EnemyContext Enemy { get; set; }
        
        public PatrolledCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(Enemy,TypeEnemyStates.Patrol);
            _stateEnemyManager.SetState(characterState, Enemy);
        }
    }
}