using Remouse.Models;

namespace Remouse.GameServer.Authorization.Implementations
{
    public class DebugAuthorizer : IAuthorizer
    {
        private int _lastId = 1;
        public AuthorizeCredentials Authorize(AuthenticationCredentials credentials, out AuthorizeResult result)
        {
            result = AuthorizeResult.Successful;
            return new AuthorizeCredentials {name = "Petya", playerId = _lastId++};
        }
    }
}