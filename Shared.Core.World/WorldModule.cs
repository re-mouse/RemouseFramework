using Remouse.DIContainer;

namespace Remouse.Shared.Core.World
{
    public class WorldModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.RegisterType<IWorldBuilder>().As<LeoEcsWorldBuilder>().WithTransientLifetime();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder) { }
    }
}