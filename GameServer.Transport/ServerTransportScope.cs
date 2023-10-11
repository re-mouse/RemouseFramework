using GameServer.Authorization;
using GameServer.Authorization.Implementations;
using GameServer.ServerTransport.Implementations;
using Shared.DIContainer;

namespace GameServer.ServerTransport
{
    public class ServerTransportScope : ContainerScope
    {
        public override void InstallScope(ContainerBuilder containerBuilder)
        {
            containerBuilder.Bind<IServerSocket>().As<LiteNetLibServer>();
            
            containerBuilder.Bind<ServerManager>().WithInterfaces();
            
            // projectContextMediator.RegisterAs<JWTAuthorizer, IAuthorizer>();
            containerBuilder.Bind<IAuthorizer>().As<DebugAuthorizer>();
        }
    }
}