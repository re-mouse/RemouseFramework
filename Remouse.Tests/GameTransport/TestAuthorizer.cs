using Cysharp.Threading.Tasks;
using Remouse.Network.Server;
using Models;
using Remouse.Network.Models;
using Remouse.Serialization;
using Remouse.Utils;

namespace Remouse.Network.Sockets.Tests
{
    public class TestAuthorizer : IAuthorizer
    {
        private string _secret = "secret1";

        public void SetSecret(string secret)
        {
            _secret = secret;
        }
        
        public async UniTask<AuthorizeResult> Authorize(AuthenticationCredentials credentials)
        {
            LLogger.Current.LogTrace(this, "Got authorize request");
            var reader = new BytesReader(credentials.authData);

            if (credentials.method == AuthenticationCredentials.Method.Debug)
            {
                var secret = reader.ReadString();
                int playerId = reader.ReadInt();

                if (secret == _secret)
                {
                    LLogger.Current.LogTrace(this, "Authorize succesfull");
                    return new AuthorizeResult(Status.Successful, new PlayerIdentity(playerId));
                }
                else
                {
                    LLogger.Current.LogTrace(this, "Authorize unsucessful");
                    return new AuthorizeResult(Status.Unsuccessful, null);
                }
            }

            LLogger.Current.LogTrace(this, "Authorize invalid data");
            return new AuthorizeResult(Status.InvalidData, null);
        }
    }
}