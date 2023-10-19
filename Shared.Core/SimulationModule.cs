using Remouse.Database;
using Remouse.Shared.Core.World;
using Remouse.DIContainer;

namespace Remouse.Shared.Core
{
    public class SimulationModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.RegisterType<SimulationFactory>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<DatabaseModule>();
            moduleBinder.RegisterModule<WorldModule>();
        }
    }
}