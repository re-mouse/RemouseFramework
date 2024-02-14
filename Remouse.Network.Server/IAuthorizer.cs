using Cysharp.Threading.Tasks;
using Remouse.Network.Models;

namespace Remouse.Network.Server
{
    public interface IAuthorizer
    {
        public UniTask<AuthorizeResult> Authorize(AuthenticationCredentials credentials);
    }

    public class AuthorizeResult
    {
        public Status Status { get; }
        public PlayerIdentity Identity { get; }
        
        public AuthorizeResult(Status status, PlayerIdentity identity)
        {
            Status = status;
            Identity = identity;
        }
    }
    
    public enum Status
    {
        Successful,
        Unsuccessful,
        InvalidData,
        InternalError,
    }
}