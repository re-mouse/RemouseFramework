using System;
using System.Net;
using GameClient.Transport;


namespace GameClient
{
    public abstract class ClientSocket
    {
        protected IClientEventsHandler? _eventsHandler { private set; get; }

        public void SetListener(IClientEventsHandler eventsHandler)
        {
            this._eventsHandler = eventsHandler;
        }
        public abstract void Connect(IPEndPoint host);
        public abstract void Disconnect();
        public abstract void Send(ReadOnlySpan<byte> data, DeliveryMethod deliveryMethod);
        public abstract void Update();
    }
}