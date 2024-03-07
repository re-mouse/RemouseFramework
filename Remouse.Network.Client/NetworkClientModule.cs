using ReDI;
using Remouse.Network.Sockets;

namespace Remouse.Network.Client
{
    public class NetworkClientModule<TAuthorizer> : Module where TAuthorizer : class, IAuthorizeProvider
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IClientTransport, ClientTransport>();
            typeBinder.AddSingleton<ClientTransportEvents>().As<IClientTransportEvents>();
            typeBinder.AddSingleton<ClientMessageBus>().As<IClientMessageBus>();
            typeBinder.AddSingleton<TAuthorizer>().As<IAuthorizeProvider>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<NetworkSocketsModule>();
        }
    }

    public class NetworkClientModule : NetworkClientModule<DummyAuthorizeProvider>
    {
    }
}