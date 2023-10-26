using System;

namespace Remouse.Core.World
{
    public interface IReadOnlyWorld
    {
        public int[] GetEntities();
        
        public Type[] GetComponentTypes();
        
        public T? GetComponent<T>(int entityId) where T : struct, IComponent;
        public bool HasComponent<T>(int entityId) where T : struct, IComponent;
        
        public object? GetComponent(Type type, int entityId);
        public bool HasComponent(Type type, int entityId);
        
        public IQueryBuilder Query();
    }
}