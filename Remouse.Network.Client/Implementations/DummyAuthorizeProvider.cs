using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.Network.Models;

namespace Remouse.Network.Client
{
    public class DummyAuthorizeProvider : IAuthorizeProvider
    {
        public async UniTask<AuthenticationCredentials> ProvideCredentialsAsync(CancellationToken cancellationToken)
        {
            return new AuthenticationCredentials("Somecode");
        }
    }
}