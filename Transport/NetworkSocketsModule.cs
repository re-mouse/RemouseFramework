using Remouse.DIContainer;
using Remouse.Transport.Implementations;

namespace Remouse.Transport
{
    public class NetworkSocketsModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddTransient<IServerSocket, LiteNetLibServer>().As<IServerSocketEvents>();

            typeBinder.AddTransient<IClientToServerSocket, LiteNetLibClientToServerSocket>().As<IClientSocketEvents>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}