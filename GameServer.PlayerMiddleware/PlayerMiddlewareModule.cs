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
            typeBinder.AddSingleton<PlayerMiddleware>().ImplementingInterfaces();
            typeBinder.AddSingleton<PlayerEventsBus>().ImplementingInterfaces();
            
            //containerBuilder.Bind<IAuthorizer>().As<JWTAuthorizer>();
            typeBinder.AddSingleton<IAuthorizer, DebugAuthorizer>(); 
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<NetworkSocketsModule>();
        }
    }
}