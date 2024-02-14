using Remouse.DI;
using Remouse.Network.Server;

namespace Remouse.GameServer
{
    public class PlayerGameIdController
    {
        private IServerTransportEvents _transportEvents;

        public void Construct(Container container)
        {
            _transportEvents = container.Resolve<IServerTransportEvents>();
            _transportEvents.Connected += HandleConnected;
        }

        private void HandleConnected(IPlayer player)
        {
            
        }
    }
}