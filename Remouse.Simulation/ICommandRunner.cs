using System.Collections;
using System.Collections.Generic;

namespace Remouse.Simulation
{
    public interface ICommandRunner
    {
        public IEnumerable<BaseWorldCommand> EnqueuedCommands { get; }
        public void EnqueueCommand(BaseWorldCommand command);
        public void RunEnqueuedCommands();
        public void RunCommands(IEnumerable<BaseWorldCommand> commands);
    }
}