using Remouse.GameServer.Authorization;
using Remouse.GameServer.Authorization.Implementations;
using Remouse.DIContainer;
using Remouse.Transport;

namespace Remouse.GameServer.ServerTransport
{
    public class PlayerMiddlewareModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder) 
        { 
            typeBinder.RegisterType<PlayerMiddleware>().ImplementingInterfaces();
            typeBinder.RegisterType<PlayerEventsBus>().ImplementingInterfaces();
            
            //containerBuilder.Bind<IAuthorizer>().As<JWTAuthorizer>();
            typeBinder.RegisterType<IAuthorizer>().As<DebugAuthorizer>(); 
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<TransportModule>();
        }
    }
}