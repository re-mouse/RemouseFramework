using ReDI;

namespace Remouse.Simulation.Network
{
    public class NetworkSimulationModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddWorldSystem<NetworkIdentityAssignerSystem>();
            typeBinder.AddSingleton<NetworkEntityRegistry>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<SimulationModule>();
        }
    }
}