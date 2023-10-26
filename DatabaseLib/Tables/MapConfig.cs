using Remouse.Core.Configs;
using Remouse.Mathlib;
using Remouse.MathLib;

namespace Remouse.DatabaseLib.Tables
{
    public class MapConfig : TableData
    {
        public TileConfig[] tiles;
        public EntityConfig[] entities;
    }
    public class EntityConfig
    {
        public TableDataLink<EntityTypeConfig> typeId;
        public ComponentConfig[] overrideComponents;
        public Vec3 position;
        public Vec4 rotation;
    }
    
    public class TileConfig
    {
        public TableDataLink<TileTypeConfig> typeId;
        public Vec2Int position;
    }
}