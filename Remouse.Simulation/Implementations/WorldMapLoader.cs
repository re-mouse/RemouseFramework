using Remouse.World;
using Remouse.Database;
using Models.Database;
using Remouse.DI;
using Remouse.Utils;

namespace Remouse.Simulation
{
    internal class WorldMapLoader
    {
        private IDatabase _database;
        private IEntityFactory _entityFactory;
        private IWorldBuilder _worldBuilder;

        public void Construct(Container container)
        {
            _database = container.Resolve<IDatabase>();
            _worldBuilder = container.Resolve<IWorldBuilder>();
            _entityFactory = container.Resolve<IEntityFactory>();
        }
        
        public IWorld Load(string mapId)
        {
            if (mapId.IsNullOrEmpty())
            {
                LLogger.Current.LogError(this , $"MapId is null");
                return null;
            }

            var world = _worldBuilder.Build();
            var mapConfig = _database.GetTableData<MapConfig>(mapId);
            if (mapConfig == null)
            {
                LLogger.Current.LogError(this, $"Map not found [MapId:{mapId}]");
            }
                
            AddEntitiesFromConfig(mapConfig, world);
            return world;
        }

        private void AddEntitiesFromConfig(MapConfig config, IWorld world)
        {
            foreach (var entityConfig in config.entities)
            {
                CreateEntity(world, entityConfig);
            }
        }

        private void CreateEntity(IWorld world, EntityMapConfig entityConfig)
        {
            var typeId = entityConfig.typeId.Id;

            int entity = _entityFactory.Create(world, typeId);

            ApplyTransformValues(world, entity, entityConfig);
        }

        private void ApplyTransformValues(IWorld world, int entityId, EntityMapConfig entityMapConfig)
        {
            ref var transform = ref world.GetOrAddComponent<ReTransform>(entityId);
            transform.position = entityMapConfig.position.ToVec2();
            transform.rotation = entityMapConfig.rotation.eulerAngles.y;
        }
    }
}