using Remouse.Serialization;

namespace Remouse.Network.Models
{
    public static class NetworkSerializeExtensions
    {
        public static void Write(this IBytesWriter writer, NetworkMessage message)
        {
            writer.WriteUShort(TypeSerializer<NetworkMessage>.GetTypeId(message));
            message.Serialize(writer);
        }

        public static NetworkMessage? ReadNetworkMessage(this IBytesReader reader)
        {
            ushort typeId = reader.ReadUShort();

            if (!TypeSerializer<NetworkMessage>.TypeExist(typeId))
                return null;

            NetworkMessage? message = TypeSerializer<NetworkMessage>.GetNew(typeId);
            
            message?.Deserialize(reader);

            return message;
        }
    }
}