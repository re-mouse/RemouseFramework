using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.Network.Client;
using Models;
using Remouse.Network.Models;
using Remouse.Serialization;
using Remouse.Utils;

namespace Remouse.Network.Sockets.Tests
{
    public class TestAuthorizeProvider : IAuthorizeProvider
    {
        private string _secret = "secret1";
        private int _playerId = 1;

        public void SetSecret(string secret)
        {
            _secret = secret;
        }
        
        public void SetPlayerId(int playerId)
        {
            _playerId = playerId;
        }
            
        public async UniTask<AuthenticationCredentials> ProvideCredentialsAsync(CancellationToken cancellationToken)
        {
            LLogger.Current.LogTrace(this, "Providing credentials");

            var writer = new BytesWriter();
            writer.WriteString(_secret);
            writer.WriteInt(_playerId);
            
            return new AuthenticationCredentials()
            {
                authData = writer.GetBytes().ToArray(),
                method = AuthenticationCredentials.Method.Debug
            };
        }
    }
}