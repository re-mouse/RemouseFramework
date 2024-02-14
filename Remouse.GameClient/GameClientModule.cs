using Remouse.DI;
using Remouse.Network.Client;
using Remouse.Simulation.Network;

namespace Remouse.GameClient
{
    public class GameClientModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<PackedSimulationListener>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<SimulationStateRequester>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<SimulationUpdatesListener>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<ReceivedWorldCommandsBuffer>().ConstructOnContainerBuild();
            typeBinder.AddSingleton<GameClientSimulationLoop>().ConstructOnContainerBuild();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<NetworkClientModule>();
            moduleBinder.RegisterModule<NetworkSimulationModule>();
        }
    }
}