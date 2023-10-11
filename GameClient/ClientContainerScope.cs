using Shared.DIContainer;
using Shared.GameSimulation;
using Shared.Utils.Log;

namespace GameClient
{
    public class ClientContainerScope : ContainerScope
    {
        public override void InstallScope(ContainerBuilder containerBuilder)
        {
            // containerBuilder.Bind<DebugDatabase>();
            
            containerBuilder.Bind<ILogger>().As<ConsoleLogger>();
            
            containerBuilder.Bind<IInputCommandsProvider>().As<InputCommandsProvider>();
            
            // containerBuilder.Bind<ClientSocket>().As<MirrorClientSocket>();
            containerBuilder.Bind<ClientSocketManager>();
            containerBuilder.Bind<ClientSocket>();
            
            containerBuilder.Bind<GameSimulationFactory>();
            containerBuilder.Bind<GameSimulationRunner>();
            containerBuilder.Bind<GameSimulationUpdatesBuffer>();
            
            containerBuilder.Bind<ClientMaster>();
        }
    }
}