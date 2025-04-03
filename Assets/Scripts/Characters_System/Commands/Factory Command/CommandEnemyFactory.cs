using Characters.Enemy;
using Characters.Command;

namespace Commands
{
    public class CommandEnemyFactory
    {
        private CommandInvoker _invoker;

        private ICommandEnemy _commandEnemyAttack;
        private ICommandEnemy _commandEnemyPatrol;
        private ICommandEnemy _commandEnemyChasing;
        private ICommandEnemy _commandEnemyDeath;
        private ICommandEnemy _commandEnemyToAttacked;
        private ICommandEnemy _commandEnemyIdle;
        private ICommandEnemy _commandEnemyAlert;
        private ICommandEnemy _commandEnemySearch;
        private ICommandEnemy _commandEnemySuspicious;
        private ICommandEnemy _commandEnemyFlee;
        
        public void Inject(DependencyContainer container)
        {
            _invoker = container.Resolve<CommandInvoker>();
            
            _commandEnemyToAttacked = new ToAttackedCommand(container);
            _commandEnemyChasing = new ChaseCommand(container);
            _commandEnemyAttack = new AttackCommand(container);
            _commandEnemyPatrol = new PatrolledCommand(container);
            _commandEnemyDeath = new DeathCommand(container);
            _commandEnemyIdle = new IdleCommand(container);
            _commandEnemyAlert = new AlertCommand(container);
            _commandEnemySearch = new SearchCommand(container);
            _commandEnemySuspicious = new SuspiciousCommand(container);
            _commandEnemyFlee = new FleeCommand(container);
        }
        
        public void CreateAttackCommand(EnemyContext enemy)
        {
            _commandEnemyAttack.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyAttack);
            _invoker.ExecuteCommands();
        }

        public void CreatePatrolledCommand(EnemyContext enemy)
        {
            _commandEnemyPatrol.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyPatrol);
            _invoker.ExecuteCommands();
        }

        public void CreateChasingCommand(EnemyContext enemy)
        {
            _commandEnemyChasing.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyChasing);
            _invoker.ExecuteCommands();
        }

        public void CharacterIdleCommand(EnemyContext enemy)
        {
            _commandEnemyIdle.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyIdle);
            _invoker.ExecuteCommands();
        }

        public void CreateDeathCommand(EnemyContext enemy)
        {
            _commandEnemyDeath.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyDeath);
            _invoker.ExecuteCommands();
        }

        public void CreateAlertCommand(EnemyContext enemy)
        {
            _commandEnemyAlert.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyAlert);
            _invoker.ExecuteCommands();
        }

        public void CreateFleeCommand(EnemyContext enemy)
        {
            _commandEnemyFlee.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyFlee);
            _invoker.ExecuteCommands();
        }

        public void CreateSearchCommand(EnemyContext enemy)
        {
            _commandEnemySearch.Enemy = enemy;
            _invoker.SetCommand(_commandEnemySearch);
            _invoker.ExecuteCommands();
        }

        public void CreateSuspiciousCommand(EnemyContext enemy)
        {
            _commandEnemySuspicious.Enemy = enemy;
            _invoker.SetCommand(_commandEnemySuspicious);
            _invoker.ExecuteCommands();
        }
        
        public void CreateToAttackedCommand(EnemyContext enemy)
        {
            _commandEnemyToAttacked.Enemy = enemy;
            _invoker.SetCommand(_commandEnemyToAttacked);
            _invoker.ExecuteCommands();
        }
    }
}