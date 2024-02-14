using Remouse.Network.Models;

namespace Remouse.Simulation.Network
{
    public abstract class BasePlayerInputMessage : NetworkMessage
    {
        public abstract BasePlayerInputCommand AsCommand();
    }
}