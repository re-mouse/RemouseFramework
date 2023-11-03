using System;
using System.Collections.Generic;
using System.Linq;
using Remouse.Core.Configs;
using Remouse.Core.Components;
using Remouse.Core.ECS.Systems;
using Remouse.Core.World;
using Remouse.DatabaseLib;
using Remouse.Models.Database;
using Remouse.DIContainer;
using Remouse.Infrastructure;
using Remouse.MathLib;
using Remouse.Shared.Utils.Log;
using Remouse.Utils;
using Tile = Remouse.Core.Components.Tile;

namespace Remouse.Core
{
    public class MapLoader
    {
        private DatabaseHost _databaseHost;
        private IWorldBuilder _worldBuilder;

        public void Construct(Container container)
        {
            _databaseHost = container.Resolve<DatabaseHost>();
            _worldBuilder = container.Resolve<IWorldBuilder>();
        }
        
        public Simulation Load(string mapId)
        {
            var world = _worldBuilder.Build();
            
            if (!mapId.IsNullOrEmpty())
            {
                var mapConfig = _databaseHost.Get().GetTableData<MapConfig>(mapId);
                if (mapConfig == null)
                {
                    Logger.Current.LogError(this, $"Map of type {mapId} not found");
                }
                AddTilesFromConfig(mapConfig, world);
                AddEntitiesFromConfig(mapConfig, world);
            }
            
            Simulation simulation = new Simulation(world);
            
            simulation.Initialize(_databaseHost.Get());

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
                        var tileTypeConfig = tileConfig.typeId.GetTableData(_databaseHost.Get());
                        
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
                var typeConfig = entityConfig.typeId.GetTableData(_databaseHost.Get());
                
                var entity = world.NewEntity();

                if (typeConfig != null && typeConfig.components != null)
                {
                    ApplyComponentConfigs(typeConfig.components, world, entity);
                }

                if (entityConfig.overrideComponents != null)
                {
                    ApplyComponentConfigs(entityConfig.overrideComponents, world, entity);
                }
            }
        }

        private void ApplyComponentConfigs(List<ComponentConfig> componentConfigs, IWorld world, int entity)
        {
            foreach (var componentConfig in componentConfigs)
            {
                if (componentConfig == null || componentConfig.componentType.IsNullOrEmpty())
                {
                    Logger.Current.LogError(this, "Empty config in map");
                    continue;
                }
                IComponent component = componentConfig.CreateComponentWithConfigValues();
                if (component == null)
                {
                    Logger.Current.LogError(this, $"Component type not found for config with name {componentConfig.componentType}" );
                    continue;
                }
                
                world.SetComponent(component.GetType(), entity, component);
            }
        }
    }
}