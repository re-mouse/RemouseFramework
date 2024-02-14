using System.Collections.Generic;
using Remouse.DI;

namespace Remouse.Simulation
{
    public class CommandRunner : ICommandRunner
    {
        private List<BaseWorldCommand> _worldCommands = new List<BaseWorldCommand>(10);
        
        public IEnumerable<BaseWorldCommand> EnqueuedCommands { get => _worldCommands; }
        private SimulationHost _simulationHost;
        private Container _container;

        public void Construct(Container container)
        {
            _container = container;
            _simulationHost = container.Resolve<SimulationHost>();
        }
        
        public void EnqueueCommand(BaseWorldCommand command)
        {
            _worldCommands.Add(command);
        }

        public void RunEnqueuedCommands()
        {
            RunCommands(_worldCommands);
            _worldCommands.Clear();
        }

        public void RunCommands(IEnumerable<BaseWorldCommand> commands)
        {
            var simulation = _simulationHost.GetSimulationInternal();
            foreach (var command in _worldCommands)
            {
                if (command is IConstructable constructableCommand)
                    constructableCommand.Construct(_container);
                
                simulation.RunCommand(command);
            }
        }
    }
}