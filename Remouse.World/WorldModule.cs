using Remouse.DI;

namespace Remouse.World
{
    public class WorldModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IWorldBuilder, LeoEcsWorldBuilder>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder) { }
    }
}