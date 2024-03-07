using ReDI;
using Remouse.Network.Client;
using Remouse.Simulation.Network;

namespace Remouse.GameClient
{
    public class GameClientModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<PackedSimulationListener>().CreateOnContainerBuild();
            typeBinder.AddSingleton<SimulationStateRequester>().CreateOnContainerBuild();
            typeBinder.AddSingleton<SimulationUpdatesListener>().CreateOnContainerBuild();
            typeBinder.AddSingleton<ReceivedWorldCommandsBuffer>().CreateOnContainerBuild();
            typeBinder.AddSingleton<GameClientSimulationLoop>().CreateOnContainerBuild();
            typeBinder.AddSingleton<GameClientIdController>().CreateOnContainerBuild();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<NetworkClientModule>();
            moduleBinder.RegisterModule<NetworkSimulationModule>();
        }
    }
}