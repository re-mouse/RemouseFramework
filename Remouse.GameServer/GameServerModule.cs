using Remouse.Database;
using ReDI;
using Remouse.Network.Server;
using Remouse.Simulation.Network;

namespace Remouse.GameServer
{
    public class GameServerModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<SimulationUpdatesController>().CreateOnContainerBuild();
            typeBinder.AddSingleton<SimulationStateController>().CreateOnContainerBuild();
            typeBinder.AddSingleton<PlayerInputController>().CreateOnContainerBuild();
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