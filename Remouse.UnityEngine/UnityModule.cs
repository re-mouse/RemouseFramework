using Remouse.DI;
using Remouse.UnityEngine.Assets;

namespace Remouse.UnityEngine
{
    public class UnityModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IGameObjectFactory, GameObjectFactory>();
            typeBinder.AddSingleton<IGameObjectsPoolManager, GameObjectsPoolManager>();
            typeBinder.AddSingleton<ICameraFactory, CameraFactory>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<AssetsModule>();
        }
    }
}