using ReDI;
using Remouse.Network.Sockets.Implementations;

namespace Remouse.Network.Sockets
{
    public class NetworkSocketsModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddTransient<IServerSocket, LiteNetLibServerSocket>();

            typeBinder.AddTransient<IClientSocket, LiteNetLibClientSocket>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}