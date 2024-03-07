using ReDI;
using Remouse.Network.Sockets;

namespace Remouse.Network.Server
{
    public class NetworkServerModule<TAuthorizer> : Module where TAuthorizer : class, IAuthorizer
    {
        public override void BindDependencies(TypeManager typeBinder) 
        { 
            typeBinder.AddSingleton<ServerTransport>().As<IServerTransport>().As<IConnectedPlayers>().CreateOnContainerBuild();
            typeBinder.AddSingleton<ServerMessageBus>().As<IServerMessageBus>().CreateOnContainerBuild();
            typeBinder.AddSingleton<ServerTransportEvents>().As<IServerTransportEvents>().CreateOnContainerBuild();
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