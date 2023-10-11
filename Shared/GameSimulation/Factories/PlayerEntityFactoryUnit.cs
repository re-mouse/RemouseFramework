using Shared.Content;
using Shared.ContentTableTypes;
using Shared.DIContainer;
using Shared.EcsLib;
using Shared.GameSimulation.ECS.Components;
using Shared.GameSimulation.ECS.Utils;
using Shared.Math;
using Transform = Shared.GameSimulation.ECS.Components.Transform;

namespace Shared.GameSimulation.Factories
{
    public class PlayerEntityFactoryUnit : IEntityFactoryUnit
    {
        private Database _database;
        private string _playerEntityId;

        public void Construct(Container container)
        {
            _database = container.Get<Database>();
            _playerEntityId = _database.GetSettings<PlayerSettings>().playerEntityId;
        }

        public int Create(EcsWorld world, string entityId)
        {
            int entity = world.NewEntity();

            ref var transform = ref EntityUtils.AddComponent<Transform>(world, entity);
            transform.position = Vec2.zero;
            
            ref var moveSpeed = ref EntityUtils.AddComponent<MovementSpeed>(world, entity);
            moveSpeed.speed = 5f;
            
            ref var moveDirection = ref EntityUtils.AddComponent<MovementDirection>(world, entity);
            moveDirection.movementDirection = Vec2.one;

            ref var networkIdentity = ref EntityUtils.AddComponent<NetworkIdentity>(world, entity);
            networkIdentity.visibility = NetworkVisibility.Default;
            
            EntityUtils.AddComponent<PlayerTag>(world, entity);
            
            return entity;
        }

        public string[] GetEntityIds()
        {
            return new string[] { _playerEntityId };
        }
    }
}