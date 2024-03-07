using Remouse.World;
using Remouse.Database;
using Models.Database;
using ReDI;
using Remouse.Utils;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    internal class WorldMapLoader
    {
        [Inject] private IDatabase _database;
        [Inject] private IEntityFactory _entityFactory;
        
        public void Load(EcsWorld world, string mapId)
        {
            if (mapId.IsNullOrEmpty())
            {
                LLogger.Current.LogError(this , $"MapId is null");
                return;
            }

            var mapConfig = _database.GetTableData<MapConfig>(mapId);
            if (mapConfig == null)
            {
                LLogger.Current.LogError(this, $"Map not found [MapId:{mapId}]");
            }
                
            AddEntitiesFromConfig(world, mapConfig);
        }

        private void AddEntitiesFromConfig(EcsWorld world, MapConfig config)
        {
            foreach (var entityConfig in config.entities)
            {
                CreateEntity(world, entityConfig);
            }
        }

        private void CreateEntity(EcsWorld world, EntityMapConfig entityConfig)
        {
            var typeId = entityConfig.typeId.Id;

            int entity = _entityFactory.Create(typeId);
            
            ApplyTransformValues(world.GetPoolOrCreate<ReTransform>(), entity, entityConfig);
        }

        private void ApplyTransformValues(EcsPool<ReTransform> pool, int entityId, EntityMapConfig entityMapConfig)
        {
            ref ReTransform transform = ref pool.AddOrGet(entityId);
            transform.position = entityMapConfig.position.ToVec2();
            transform.rotation = entityMapConfig.rotation.eulerAngles.y;
        }
    }
}