using System;
using System.Collections.Generic;
using System.Linq;
using Remouse.Core.Configs;
using Remouse.Core.Components;
using Remouse.Core.ECS.Systems;
using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.DatabaseLib.Tables;
using Remouse.DIContainer;
using Remouse.Mathlib;
using Remouse.MathLib;

namespace Remouse.Core
{
    public class SimulationFactory
    {
        private Database _database;
        private IWorldBuilder _worldBuilder;

        public void Construct(Container container)
        {
            _database = container.Resolve<Database>();
            _worldBuilder = container.Resolve<IWorldBuilder>();
            
            RegisterTypesInWorldBuilder();
        }
        
        private void RegisterTypesInWorldBuilder()
        {
            _worldBuilder.RegisterSystem<WaypointSystem>();
            _worldBuilder.RegisterSystem<PositionTeleportSystem>();
            _worldBuilder.RegisterSystem<MoveToDirectionSystem>();
            
            _worldBuilder.RegisterComponentType<TilesSingleton>();
            _worldBuilder.RegisterComponentType<MoveInDirection>();
            _worldBuilder.RegisterComponentType<MovementSpeed>();
            _worldBuilder.RegisterComponentType<NetworkIdentity>();
            _worldBuilder.RegisterComponentType<PlayerTag>();
            _worldBuilder.RegisterComponentType<PositionTeleport>();
            _worldBuilder.RegisterComponentType<ReTransform>();
            _worldBuilder.RegisterComponentType<WaypointPath>();
        }
        
        public Simulation CreateGameSimulation(string mapId)
        {
            var mapConfig = _database.GetTableData<MapConfig>(mapId);
            var world = _worldBuilder.Build();
            
            AddTilesFromConfig(mapConfig, world);
            AddEntitiesFromConfig(mapConfig, world);
            
            Simulation simulation = new Simulation(world);
            
            simulation.Initialize();

            return simulation;
        }

        private void AddTilesFromConfig(MapConfig config, IWorld world)
        {
            int entity = world.NewEntity();

            int maxX = config.tiles.Max(t => t.position.x) + 1;
            int maxY = config.tiles.Max(t => t.position.y) + 1;
            
            var tiles = new Tile[maxX][];
            for (var iX = 0; iX < tiles.Length; iX++)
            {
                tiles[iX] = new Tile[maxY];

                foreach (var tileConfig in config.tiles)
                {
                    if (tileConfig.position.x == iX)
                    {
                        ref var tile = ref tiles[iX][tileConfig.position.y];
                        var tileTypeConfig = tileConfig.typeId.GetTableData(_database);
                        
                        tile.walkable = tileTypeConfig.type == TileType.Walkable;
                    }
                }
            }
            
            ref var tilesComponent = ref world.AddComponent<TilesSingleton>(entity);
            tilesComponent.tiles = tiles;
        }

        private void AddEntitiesFromConfig(MapConfig config, IWorld world)
        {
            foreach (var entityConfig in config.entities)
            {
                var typeConfig = entityConfig.typeId.GetTableData(_database);
                
                var entity = world.NewEntity();

                ApplyComponentConfigs(typeConfig.components, world, entity);
                ApplyComponentConfigs(entityConfig.overrideComponents, world, entity);
                
                ref var transform = ref  world.AddComponent<ReTransform>(entity);
                transform.position = new Vec2(entityConfig.position.x, entityConfig.position.z);
                transform.rotationAngle = entityConfig.rotation.y;
            }
        }

        private void ApplyComponentConfigs(ComponentConfig[] componentConfigs, IWorld world, int entity)
        {
            foreach (var componentConfig in componentConfigs)
            {
                IComponent component = componentConfig.Create(world);
                world.SetComponent(component.GetType(), entity, component);
            }
        }
    }
}