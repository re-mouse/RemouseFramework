namespace Remouse.Core.World
{
    public interface IPackedEntity
    {
        public int EntityId { get; }
        public bool IsAlive();
    }
}