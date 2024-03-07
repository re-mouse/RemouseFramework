using ReDI;
using Remouse.Network.Sockets.Implementations;

namespace Remouse.Network.Sockets
{
    public class NetworkSocketsModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddTransient<IServerSocket, LiteNetLibServer>().As<IServerSocketEvents>();

            typeBinder.AddTransient<IClientSocket, LiteNetLibClientSocket>().As<IClientSocketEvents>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}