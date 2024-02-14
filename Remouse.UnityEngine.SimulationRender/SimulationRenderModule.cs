using Remouse.DI;
using Remouse.UnityEngine.Assets;

namespace Remouse.UnityEngine.SimulationRender
{
    public class SimulationRenderModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<ISimulationRenderer, SimulationRenderer>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<AssetsModule>();
        }
    }
}