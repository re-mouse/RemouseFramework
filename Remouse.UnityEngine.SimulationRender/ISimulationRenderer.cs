using Cysharp.Threading.Tasks;

namespace Remouse.UnityEngine.SimulationRender
{
    public interface ISimulationRenderer
    {
        public void Render();
        public void Clear();
        public UniTask PrepareToRenderEntity(string typeId);
    }
}