namespace Remouse.Network.Sockets;

public class ServerWebSocket : IServerSocket
{
    public event Action<ConnectionRequest>? GotConnectionRequest;
    public event Action<Connection>? Connected;
    public event Action<Connection>? Disconnected;
    public event Action<Connection, byte[]>? DataReceived;

    public ServerWebSocket()
    {
        
    }
    
    public void Start(ushort port, int maxConnections)
    {
    }

    public void Stop() 
    {
    }

    public void Dispose()
    {
    }
}