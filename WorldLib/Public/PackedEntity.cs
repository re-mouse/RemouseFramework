using Remouse.Shared.EcsLib.LeoEcsLite;

namespace Remouse.Core.World
{
    public struct PackedEntity : IPackedEntity
    {
        private readonly EcsWorld _ecsWorld;
        private readonly int _entityId;
        private readonly short _gen;

        public int EntityId { get => _entityId; }
        
        internal PackedEntity(EcsWorld ecsWorld, int entityId)
        {
            _ecsWorld = ecsWorld;
            _entityId = entityId;
            _gen = ecsWorld.GetEntityGen(entityId);
        }


        public bool IsAlive()
        {
            return _ecsWorld != null
                   && _ecsWorld.IsAlive ()
                   && _ecsWorld.IsEntityAliveInternal (_entityId)
                   && _ecsWorld.GetEntityGen (_entityId) == _gen;
        }
    }
}