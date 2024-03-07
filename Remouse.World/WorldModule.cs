using ReDI;

namespace Remouse.World
{
    public class WorldModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<EcsSystemsBuilder>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder) { }
    }
}