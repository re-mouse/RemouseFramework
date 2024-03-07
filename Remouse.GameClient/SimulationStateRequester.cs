using ReDI;
using Remouse.Network.Client;
using Remouse.Network.Sockets;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class SimulationStateRequester
    {
        [Inject] private IClientTransport _clientTransport;

        public void RequestPackedSimulation()
        {
            _clientTransport.Send(new SimulationStateRequestMessage(), DeliveryMethod.ReliableUnordered);
        }
    }
}