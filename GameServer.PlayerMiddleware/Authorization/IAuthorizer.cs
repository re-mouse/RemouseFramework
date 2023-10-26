using Remouse.Models;

namespace Remouse.GameServer.Authorization
{
    public interface IAuthorizer
    {
        public AuthorizeCredentials Authorize(AuthenticationCredentials credentials, out AuthorizeResult result);
    }

    public enum AuthorizeResult
    {
        Successful,
        AuthorizeDataExpired,
        InvalidData, 
        InternalError,
        UserNotFound,
    }

    public class AuthorizeCredentials
    {
        public int playerId;
        public string name;
    }
}