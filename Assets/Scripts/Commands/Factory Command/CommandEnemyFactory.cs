using Characters.Enemy;
using Characters.Command;

namespace Commands
{
    public class CommandEnemyFactory
    {
        private CommandInvoker _invoker;
        private DependencyContainer _container;

        public void Inject(DependencyContainer container)
        {
            _invoker = container.Resolve<CommandInvoker>();
            _container = container;
        }
        
        public void CreateAttackCommand(IEnemy enemy)
        {
            ICommand attackCommand = new AttackCommand(_container, enemy);
            _invoker.SetCommand(attackCommand);
            _invoker.ExecuteCommands();
        }

        public void CreatePatrolledCommand(IEnemy enemy)
        {
            ICommand patrolledCommand = new PatrolledCommand(_container, enemy);
            _invoker.SetCommand(patrolledCommand);
            _invoker.ExecuteCommands();
        }

        public void CreateChasingCommand(IEnemy enemy)
        {
            ICommand chasingCommand = new ChasingCommand(_container, enemy);
            _invoker.SetCommand(chasingCommand);
            _invoker.ExecuteCommands();
        }

        public void CharacterIdleCommand(IEnemy enemy)
        {
            ICommand idleCommand = new IdleCommand(_container, enemy);
            _invoker.SetCommand(idleCommand);
            _invoker.ExecuteCommands();
        }

        public void CreateDeathCommand(IEnemy enemy)
        {
            ICommand deathCommand = new DeathCommand(_container, enemy);
            _invoker.SetCommand(deathCommand);
            _invoker.ExecuteCommands();
        }
    }
}