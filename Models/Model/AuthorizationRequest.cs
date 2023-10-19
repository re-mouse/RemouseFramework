using System;
using Remouse.Serialization;

namespace Remouse.Models
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