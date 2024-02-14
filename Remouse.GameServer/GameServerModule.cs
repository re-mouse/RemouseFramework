using Remouse.Database;
using Remouse.DI;
using Remouse.Network.Server;
using Remouse.Simulation.Network;

namespace Remouse.GameServer
{
    public class GameServerModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<SimulationUpdatesController>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<SimulationStateController>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<PlayerInputController>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<GameServerLoop>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<DatabaseModule>();
            moduleBinder.RegisterModule<NetworkServerModule>();
            moduleBinder.RegisterModule<NetworkSimulationModule>();
        }
    }
}