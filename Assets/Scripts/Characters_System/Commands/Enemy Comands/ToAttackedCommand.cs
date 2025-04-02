using Characters.Character_Interfaces;
using Characters.Enemy;
using Characters.Player;

namespace Characters.Command
{
    public class ToAttackedCommand: ICommandEnemy
    {
        private readonly StateEnemyManager _stateEnemyManager;
        private readonly StateEnemyFactory _stateEnemyFactory;
        private readonly ITargetHandler _targetHandler;
        public EnemyContext Enemy {get; set;}
        
        public ToAttackedCommand(DependencyContainer container)
        {
            _stateEnemyManager = container.Resolve<StateEnemyManager>();
            _stateEnemyFactory = container.Resolve<StateEnemyFactory>();
            _targetHandler = container.Resolve<PlayerContext>();
        }

        public void Execute()
        {
            Enemy.VisionChecker.SetTargetHandler(_targetHandler);
            var characterState = _stateEnemyFactory.CreateState(Enemy,TypeEnemyStates.Chased);
            _stateEnemyManager.SetState(characterState, Enemy);
        }
    }

}