namespace GameClient
{
    public interface IClientEventsHandler
    {
        public void HandleConnected();
        public void HandleDisconnected();
        public void HandleData(INetworkReader data);
    }
}