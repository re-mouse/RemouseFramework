using Remouse.DIContainer;

namespace Remouse.Core.World
{
    public class WorldModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddTransient<IWorldBuilder, LeoEcsWorldBuilder>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder) { }
    }
}