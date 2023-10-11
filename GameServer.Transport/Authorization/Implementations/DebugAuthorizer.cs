namespace GameServer.Authorization.Implementations
{
    public class DebugAuthorizer : IAuthorizer
    {
        private int lastId = 1;
        public AuthorizeCredentials Authorize(byte[] authorizeData, out AuthorizeResult result)
        {
            result = AuthorizeResult.Successful;
            return new AuthorizeCredentials() {name = "Petya", playerId = lastId++};
        }
    }
}