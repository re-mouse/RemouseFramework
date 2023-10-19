using Remouse.Shared.Core;
using Remouse.Shared.DIContainer;
using Remouse.Shared.Utils.Log;

namespace GameClient
{
    public static class ClientContainerExtensions 
    {
        public static void InstallClient(this ContainerBuilder containerBuilder)
        {
            // containerBuilder.Bind<DebugDatabase>();
            
            containerBuilder.Bind<ILogger>().As<ConsoleLogger>();
            
            containerBuilder.Bind<IInputCommandsProvider>().As<InputCommandsProvider>();
            
            // containerBuilder.Bind<ClientSocket>().As<MirrorClientSocket>();
            containerBuilder.Bind<ClientSocketManager>();
            containerBuilder.Bind<ClientSocket>();
            
            containerBuilder.Bind<SimulationFactory>();
            containerBuilder.Bind<GameSimulationRunner>();
            containerBuilder.Bind<GameSimulationUpdatesBuffer>();
            
            containerBuilder.Bind<ClientMaster>();
        }
    }
}