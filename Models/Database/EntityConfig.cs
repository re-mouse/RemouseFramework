using System;
using Remouse.Core.Configs;

namespace Remouse.Models.Database
{
    [Serializable]
    public class EntityConfig
    {
        public TableDataLink<EntityTypeConfig> typeId;
        public ComponentConfig[] overrideComponents;
    }
}