namespace Remouse.Network.Server
{
    public interface IServerTransport
    {
        void Start(ushort port, int maxPlayers);
        void Stop();
        void Update();
    }
}