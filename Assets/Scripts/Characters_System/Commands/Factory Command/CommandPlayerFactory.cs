using Characters;
using Characters.Command;
using Characters.Player;

namespace Commands
{
    public class CommandPlayerFactory
    {
        private CommandInvoker _invoker;
        private DependencyContainer _container;

        
        private ICommandPlayer _deadCommand;
        private ICommandPlayer _regularCommand;
        private ICommandPlayer _combatCommand;
        private ICommandPlayer _baseCommand;
        
        public void Inject(DependencyContainer container)
        {
            _invoker = container.Resolve<CommandInvoker>();
            _container = container;
            
            _deadCommand = new DeadCommand(_container);
            _regularCommand = new RegularCommand(_container);
            _combatCommand = new CombatCommand(_container);
            _baseCommand = new BaseStateCommand(_container);
        }
        
        public void CreateDeadCommand(PlayerContext player)
        {
            _deadCommand.Player = player;
            _invoker.SetCommand(_deadCommand);
            _invoker.ExecuteCommands();
        }

        public void CreateRegularCommand(PlayerContext player)
        {
            _regularCommand.Player = player;
            _invoker.SetCommand(_regularCommand);
            _invoker.ExecuteCommands();
        }

        public void CreateCombatCommand(PlayerContext player)
        {
            _combatCommand.Player = player;
            _invoker.SetCommand(_combatCommand);
            _invoker.ExecuteCommands();
        }

        public void CreateBaseState(PlayerContext player)
        {
            _baseCommand.Player = player;
            _invoker.SetCommand(_baseCommand);
            _invoker.ExecuteCommands();
        }
    }
}