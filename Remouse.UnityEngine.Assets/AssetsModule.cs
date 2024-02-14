using Remouse.DI;

namespace Remouse.UnityEngine.Assets
{
    public class AssetsModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IAssetLoader, AddressableAssetLoader>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}