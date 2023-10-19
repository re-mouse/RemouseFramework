using System;
using Remouse.Shared.Serialization;

namespace Remouse.Shared.Models
{
    public struct AuthorizationRequest : INetworkSerializable
    {
        public byte[] authData;
        public Method method;
        
        public enum Method
        {
            Debug,
            JWT
        };

        public void Serialize(INetworkWriter writer)
        {
            writer.WriteByteArray(authData);
            writer.WriteString(method.ToString());
        }

        public void Deserialize(INetworkReader reader)
        {
            authData = reader.ReadByteArray();
            method = Enum.Parse<Method>(reader.ReadString());
        }
    }
}