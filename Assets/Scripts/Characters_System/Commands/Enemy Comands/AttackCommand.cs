using Characters.Enemy;

namespace Characters.Command
{
    public class AttackCommand : ICommandEnemy
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        public IEnemy Enemy { get; set; }
        
        public AttackCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
        }

        public void Execute()
        {
            var characterState = _stateEnemyFactory.CreateState(Enemy,TypeEnemyStates.Attacked);
            _stateEnemyManager.SetState(characterState, Enemy);
        }
    }
}
