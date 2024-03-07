using Remouse.World;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public interface IEntityFactory
    {
        public int Create();
        public int Create(string typeId);
    }
}