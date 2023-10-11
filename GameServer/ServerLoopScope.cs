using GameServer.InterestManagment;
using GameServer.Players;
using GameServer.ServerShards;
using Shared.DIContainer;
using Shared.GameSimulation;

namespace GameServer
{
    public class ServerLoopScope : ContainerScope
    {
        public override void InstallScope(ContainerBuilder containerBuilder)
        {
            containerBuilder.Bind<GameSimulationFactory>();
            containerBuilder.Bind<GameSimulationHolder>();

            containerBuilder.Bind<PlayerInputBuffer>();
            
            containerBuilder.Bind<EntityUpdatesSender>();
            containerBuilder.Bind<EntityUpdatesTracker>();
            containerBuilder.Bind<NetworkEntityRegistry>();
            containerBuilder.Bind<PlayerObservingRegistry>();
            containerBuilder.Bind<IEntityObservingStrategy>().As<GridBasedObservingStrategy>();
            
            containerBuilder.Bind<PlayerManager>().WithInterfaces();
        }
    }
}