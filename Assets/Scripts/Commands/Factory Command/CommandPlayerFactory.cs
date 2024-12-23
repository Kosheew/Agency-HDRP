using Characters;
using Characters.Command;

namespace Commands
{
    public class CommandPlayerFactory
    {
        private CommandInvoker _invoker;
        private DependencyContainer _container;

        public void Inject(DependencyContainer container)
        {
            _invoker = container.Resolve<CommandInvoker>();
            _container = container;
        }
        
        public void CreateDeadCommand(IPlayer player)
        {
            ICommand deadCommand = new DeadCommand(_container, player);
            _invoker.SetCommand(deadCommand);
            _invoker.ExecuteCommands();
        }

        public void CreateMoveCommand(IPlayer player)
        {
            ICommand moveCommand = new RegularCommand(_container, player);
            _invoker.SetCommand(moveCommand);
            _invoker.ExecuteCommands();
        }
    }
}