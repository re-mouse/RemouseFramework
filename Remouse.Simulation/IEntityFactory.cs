using Remouse.World;

namespace Remouse.Simulation
{
    public interface IEntityFactory
    {
        public int Create(IWorld world);
        public int Create(IWorld world, string typeId);
    }
}