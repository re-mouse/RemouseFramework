using Remouse.DI;
using Remouse.Network.Client;
using Remouse.Network.Sockets;
using Remouse.Simulation.Network;
using Remouse.Utils;

namespace Remouse.GameClient
{
    public class SimulationStateRequester
    {
        private IClientTransport _clientTransport;

        public void Construct(Container container)
        {
            _clientTransport = container.Resolve<IClientTransport>();
        }

        public void RequestPackedSimulation()
        {
            _clientTransport.Send(new SimulationStateRequestMessage(), DeliveryMethod.ReliableUnordered);
        }
    }
}