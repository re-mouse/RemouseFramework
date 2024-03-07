using System.Collections.Generic;
using System.Linq;
using ReDI;

namespace Remouse.Simulation
{
    public class CommandRunner : ICommandRunner
    {
        private List<BaseWorldCommand> _worldCommands = new List<BaseWorldCommand>(10);
        private List<BaseWorldCommand> _worldCommandsBuffer = new List<BaseWorldCommand>(10);

        public IEnumerable<BaseWorldCommand> EnqueuedCommands { get => _worldCommandsBuffer; }
        
        [Inject] private SimulationHost _simulationHost;
        
        public bool IsEmpty()
        {
            return _worldCommandsBuffer.Count == 0;
        }
        
        public void EnqueueCommand(BaseWorldCommand command)
        {
            _worldCommandsBuffer.Add(command);
        }

        public void RunEnqueuedCommands()
        {
            var commandsBuffer = _worldCommandsBuffer;
            _worldCommandsBuffer = _worldCommands;
            _worldCommands = commandsBuffer;
            RunCommands(_worldCommands);
            _worldCommands.Clear();
        }

        public void RunCommands(IEnumerable<BaseWorldCommand> commands)
        {
            var simulation = _simulationHost.GetSimulationInternal();
            foreach (var command in commands)
            {
                simulation.RunCommand(command);
            }
        }
    }
}