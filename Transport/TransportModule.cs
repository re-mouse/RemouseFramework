using Remouse.DIContainer;
using Remouse.Transport.Implementations;

namespace Remouse.Transport
{
    public class TransportModule : Module
    {
        private readonly bool _singleton;

        public TransportModule()
        {
            
        }

        public TransportModule(bool singleton) { _singleton = singleton; }
        
        public override void BindDependencies(TypeManager typeBinder)
        {
            if (_singleton)
            {
                typeBinder.AddSingleton<IServerSocket, LiteNetLibServer>().As<IServerSocketEvents>();

                typeBinder.AddSingleton<IClientToServerSocket, LiteNetLibClientToServerSocket>().As<IClientSocketEvents>();
            }
            else
            {
                typeBinder.AddTransient<IServerSocket, LiteNetLibServer>().As<IServerSocketEvents>();

                typeBinder.AddTransient<IClientToServerSocket, LiteNetLibClientToServerSocket>().As<IClientSocketEvents>();
            }
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}