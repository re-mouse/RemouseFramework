using Remouse.Serialization;

namespace Remouse.Models.Messages.Authentification
{
    public class AuthenticationRequestMessage : NetworkMessage
    {
        public AuthenticationCredentials credentialsData;

        public override void Serialize(INetworkWriter writer)
        {
            credentialsData.Serialize(writer);
        }

        public override void Deserialize(INetworkReader reader)
        {
            credentialsData.Deserialize(reader);
        }
    }
}