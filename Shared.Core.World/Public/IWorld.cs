using System;

namespace Remouse.Shared.Core.World
{
    public interface IWorld : IReadOnlyWorld
    {
        public void Tick();
        public void Initialize();
        public ref T GetComponentRef<T>(int entityId) where T : struct;
        public ref T AddComponent<T>(int entityId) where T : struct;
        public void DelComponent<T>(int entityId) where T : struct; 
        public int NewEntity();
        public void DelEntity(int entityId);
    }
    
    public interface IReadOnlyWorld
    {
        public int[] GetEntities();
        public Type[] GetComponentTypes();
        public object? GetComponent(Type type, int entityId);
        public T GetComponent<T>(int entityId) where T : struct, IComponent;
        public bool HasComponent<T>(int entityId) where T : struct;
        public bool HasComponent(Type type, int entityId);
        public IQueryBuilder Query();
    }
}