using System;
using Remouse.Serialization;

namespace Remouse.Network.Models
{
    public class AuthenticationCredentials : IBytesSerializable
    {
        public byte[] authData;
        public Method method;
        
        public enum Method
        {
            Debug,
            JWT
        };

        public void Serialize(IBytesWriter writer)
        {
            writer.WriteByteArray(authData);
            writer.WriteString(method.ToString());
        }

        public void Deserialize(IBytesReader reader)
        {
            authData = reader.ReadByteArray();
            method = Enum.Parse<Method>(reader.ReadString());
        }
    }
}