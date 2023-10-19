using Remouse.DIContainer;
using Remouse.GameClient.Transport;
using Remouse.Transport;
using Remouse.Transport.Implementations;

namespace Remouse.Transport
{
    public class TransportModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.RegisterType<LiteNetLibServer>().As<IServerSocket>().As<IServerSocketEvents>().WithTransientLifetime();
            
            typeBinder.RegisterType<LiteNetLibClientSocket>().As<IClientSocketEvents>().As<IClientSocket>().WithTransientLifetime();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}