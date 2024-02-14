using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.Network.Models;

namespace Remouse.Network.Client
{
    public interface IAuthorizeProvider
    {
        public UniTask<AuthenticationCredentials> ProvideCredentialsAsync(CancellationToken cancellationToken);
    }
}