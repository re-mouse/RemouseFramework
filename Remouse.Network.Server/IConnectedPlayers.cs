using System.Collections.Generic;

namespace Remouse.Network.Server
{
    public interface IConnectedPlayers
    {
        IEnumerable<IPlayer> Players { get; }
    }
}