using Remouse.DI;
using Remouse.Network.Sockets;

namespace Remouse.Network.Server
{
    public class NetworkServerModule<TAuthorizer> : Module where TAuthorizer : class, IAuthorizer
    {
        public override void BindDependencies(TypeManager typeBinder) 
        { 
            typeBinder.AddSingleton<ServerTransport>().As<IServerTransport>().As<IConnectedPlayers>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<ServerMessageBus>().As<IServerMessageBus>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<ServerTransportEvents>().As<IServerTransportEvents>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<TAuthorizer>().As<IAuthorizer>(); 
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<NetworkSocketsModule>();
        }
    }

    public class NetworkServerModule : NetworkServerModule<DummyAuthorizer>
    {
    }
}