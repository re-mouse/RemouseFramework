using Remouse.GameServer.Players;

namespace Remouse.GameServer.ServerTransport
{
    public interface IPlayerServer : IConnectedPlayers
    {
        public void Start(ushort port, int maxPlayers);
        public void Stop();
        public void Update();
    }
}