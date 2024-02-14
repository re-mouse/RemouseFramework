namespace Remouse.World
{
    public interface IWorldBuilder
    {
        public void AddSystem<T>(int priority = 0) where T : class, ISystem, new();
        public void AddSystem<T>(T t, int priority = 0) where T : class, ISystem, new();
        public IWorld Build();
    }
}