using GameClient.Transport;
using Shared.Online.Commands;

namespace GameClient
{
    interface IClientCommandsSender
    {
        void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.Sequenced);
    }
}