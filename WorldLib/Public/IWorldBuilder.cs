namespace Remouse.Core.World
{
    public interface IWorldBuilder
    {
        public void RegisterComponentType<T>() where T : struct, IComponent;
        public void RegisterSystem<T>() where T : class, ISystem, new();
        public IWorld Build();
    }
}