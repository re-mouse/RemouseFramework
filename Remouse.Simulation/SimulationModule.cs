using Remouse.World;
using Remouse.Database;
using Remouse.DI;

namespace Remouse.Simulation
{
    public class SimulationModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<WorldMapLoader>();
            typeBinder.AddSingleton<SimulationHost>().As<ISimulationHost>();
            typeBinder.AddSingleton<IEntityFactory, EntityFactory>();
            typeBinder.AddSingleton<ISimulationLoader, SimulationLoader>();
            typeBinder.AddSingleton<ISimulationSerializer, SimulationSerializer>();
            typeBinder.AddSingleton<ICommandRunner, CommandRunner>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<DatabaseModule>();
            moduleBinder.RegisterModule<WorldModule>();
        }
    }
}