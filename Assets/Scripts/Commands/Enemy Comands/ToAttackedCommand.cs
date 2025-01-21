using Characters.Character_Interfaces;
using Characters.Enemy;

namespace Characters.Command
{
    public class ToAttackedCommand: ICommand
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        private readonly IEnemy _enemy;
        private readonly ITargetHandler _targetHandler;
        
        public ToAttackedCommand(DependencyContainer container, IEnemy enemy)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
            _targetHandler = container.Resolve<IPlayer>();
            _enemy = enemy;
        }

        public void Execute()
        {
            _enemy.VisionChecker.SetTargetHandler(_targetHandler);
            var characterState = _stateEnemyFactory.CreateState(TypeCharacterStates.Chased);
            _stateEnemyManager.SetState(characterState, _enemy);
        }
    }

}