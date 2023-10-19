using System;
using System.Net;
using GameClient.Transport;

namespace GameClient
{
    public abstract class ClientSocket
    {
        protected IClientEventsHandler? EventsHandler { private set; get; }

        public void SetListener(IClientEventsHandler eventsHandler)
        {
            EventsHandler = eventsHandler;
        }
        public abstract void Connect(IPEndPoint host);
        public abstract void Disconnect();
        public abstract void Send(ReadOnlySpan<byte> data, DeliveryMethod deliveryMethod);
        public abstract void Update();
    }
}