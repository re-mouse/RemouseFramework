using System.Collections.Generic;
using Remouse.Database;
using Models.Database;
using Remouse.UnityEngine.SimulationRender;
using Remouse.UnityEngine.Utils;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class SceneMapExporterTool
    {
        private EntityInstallerComponent[] _entityComponents;
        
        public MapConfig GetCurrentSceneAsConfig(string mapId)
        {
            _entityComponents = Object.FindObjectsOfType<EntityInstallerComponent>();
            
            return new MapConfig()
            {
                id = mapId,
                entities = CreateEntityConfigs(),
            };
        }

        private List<EntityMapConfig> CreateEntityConfigs()
        {
            var entities = new List<EntityMapConfig>(_entityComponents.Length);

            for (int i = 0; i < _entityComponents.Length; i++)
            {
                var entity = _entityComponents[i];
                
                var config = new EntityMapConfig();
                
                config.typeId = new TableDataLink<EntityTypeConfig>() { Id = entity.typeId };
                config.position = entity.transform.position.ToVec3();
                config.rotation = entity.transform.rotation.ToVec4();
                config.scale = entity.transform.lossyScale.ToVec3();
                
                entities.Add(config);
            }

            return entities;
        }
    }
}