using Remouse.GameServer.Authorization;
using Remouse.GameServer.Authorization.Implementations;
using Remouse.GameServer.ServerTransport.Implementations;
using Remouse.DIContainer;

namespace Remouse.GameServer.ServerTransport
{
    public class GameServerTransportModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder) 
        { 
            typeBinder.RegisterType<IServerSocket>().As<LiteNetLibServer>();
            
            typeBinder.RegisterType<ServerManager>().ImplementingInterfaces();
            typeBinder.RegisterType<ServerEventsBus>().ImplementingInterfaces();
            
            //containerBuilder.Bind<IAuthorizer>().As<JWTAuthorizer>();
            typeBinder.RegisterType<IAuthorizer>().As<DebugAuthorizer>(); 
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            //we don't use anything, only utils or types
            //and we don't have to depend on something, everything have to be incapsulated in this module
        }
    }
}