using Cysharp.Threading.Tasks;
using Remouse.Network.Models;

namespace Remouse.Network.Server
{
    public class DummyAuthorizer : IAuthorizer
    {
        private int _lastId = 1;
        
        public async UniTask<AuthorizeResult> Authorize(AuthenticationCredentials credentials)
        {
            return new AuthorizeResult(Status.Successful, new PlayerIdentity (_lastId++ ));
        }
    }
}