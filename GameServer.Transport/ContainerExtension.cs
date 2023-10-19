using Remouse.GameServer.Authorization;
using Remouse.GameServer.Authorization.Implementations;
using Remouse.GameServer.ServerTransport.Implementations;
using Remouse.Shared.DIContainer;

namespace Remouse.GameServer.ServerTransport
{
    public static class ContainerExtension
    {
        public static void InstallServerTransport(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Bind<IServerSocket>().As<LiteNetLibServer>();
            
            containerBuilder.Bind<ServerManager>().WithInterfaces();
            containerBuilder.Bind<ServerEventsBus>().WithInterfaces();
            
            // projectContextMediator.RegisterAs<JWTAuthorizer, IAuthorizer>();
            containerBuilder.Bind<IAuthorizer>().As<DebugAuthorizer>();
        }
    }
}