using System.Collections.Generic;
using Remouse.GameServer.ServerTransport;

namespace Remouse.GameServer.Players
{
    public interface IPlayersProvider
    {
        public IEnumerable<IPlayerConnection> Players { get; }
    }
}