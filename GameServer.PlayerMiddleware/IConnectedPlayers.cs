using System.Collections.Generic;
using Remouse.GameServer.ServerTransport;

namespace Remouse.GameServer.Players
{
    public interface IConnectedPlayers
    {
        public IEnumerable<IPlayer> Players { get; }
    }
}