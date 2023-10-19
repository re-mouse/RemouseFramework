using GameClient.Transport;
using Remouse.Shared.Models.Messages;

namespace GameClient
{
    interface IClientCommandsSender
    {
        void Send(NetworkMessage message, DeliveryMethod deliveryMethod = DeliveryMethod.Sequenced);
    }
}