using Remouse.DI;

namespace Remouse.UnityEngine.UI
{
    public class UIModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IUIViewFactory, UIViewFactory>();
            typeBinder.AddSingleton<ICanvasDispatcher, CanvasDispatcher>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}