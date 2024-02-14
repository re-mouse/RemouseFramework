using System;
using System.Collections.Generic;
using Remouse.Database;
using Remouse.Math;

namespace Models.Database
{
    [Serializable]
    public class MapConfig : TableData
    {
        public List<EntityMapConfig> entities;
    }
    
    [Serializable]
    public class EntityMapConfig
    {
        public TableDataLink<EntityTypeConfig> typeId;
        public Vec3 position;
        public Vec3 scale;
        public Vec4 rotation;
    }
}