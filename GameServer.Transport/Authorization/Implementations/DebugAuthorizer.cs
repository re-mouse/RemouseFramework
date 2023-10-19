namespace Remouse.GameServer.Authorization.Implementations
{
    public class DebugAuthorizer : IAuthorizer
    {
        private int _lastId = 1;
        public AuthorizeCredentials Authorize(byte[] authorizeData, out AuthorizeResult result)
        {
            result = AuthorizeResult.Successful;
            return new AuthorizeCredentials {name = "Petya", playerId = _lastId++};
        }
    }
}