using Remouse.Serialization;

namespace Remouse.Network.Models
{
    public class AuthenticationRequest : NetworkMessage
    {
        public AuthenticationCredentials credentialsData;

        public override void Serialize(IBytesWriter writer)
        {
            credentialsData.Serialize(writer);
        }

        public override void Deserialize(IBytesReader reader)
        {
            credentialsData.Deserialize(reader);
        }
    }
}