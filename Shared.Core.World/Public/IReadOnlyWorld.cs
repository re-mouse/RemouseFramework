using System;

namespace Remouse.Shared.Core.World
{
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