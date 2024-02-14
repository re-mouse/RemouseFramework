using System;

namespace Remouse.World
{
    public interface IReadOnlyWorld
    {
        public Type[] ComponentTypes { get; }
        
        public int[] GetEntities();
        public IPackedEntity PackEntity(int entityId);
        
        public T GetComponent<T>(int entityId) where T : struct, IComponent;
        public bool HasComponent<T>(int entityId) where T : struct, IComponent;
        
        public object GetComponent(Type type, int entityId);
        public bool HasComponent(Type type, int entityId);
        
        public IQueryBuilder Query();
    }
}