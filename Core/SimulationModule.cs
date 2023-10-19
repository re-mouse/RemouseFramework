using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.DIContainer;

namespace Remouse.Core
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