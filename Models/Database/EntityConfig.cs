using System;
using System.Collections.Generic;
using Remouse.Core.Configs;

namespace Remouse.Models.Database
{
    [Serializable]
    public class EntityConfig
    {
        public TableDataLink<EntityTypeConfig> typeId;
        public List<ComponentConfig> overrideComponents;
    }
}