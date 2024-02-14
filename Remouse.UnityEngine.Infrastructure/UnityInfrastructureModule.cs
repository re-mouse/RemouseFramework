using Infrastructure;
using Remouse.DI;
using Remouse.Utils;

namespace Remouse.UnityEngine.Assets
{
    public class UnityInfrastructureModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IResources, UnityResourceProvider>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}