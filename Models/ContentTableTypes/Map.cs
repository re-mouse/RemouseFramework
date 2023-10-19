using Remouse.DatabaseLib;
using Remouse.MathLib;

namespace Remouse.Models.ContentTableTypes
{
    public class Map : TableData
    {
        public TileInfo[] tileInfos;
        public MapEntity[] mapEntities;
    }

    public class TileInfo
    {
        public string tileId;
        public int x;
        public int y;
        public int z;
        public bool isWalkable;
    }

    public class Tile : TableData
    {
        public bool isWalkable;
    }

    public class MapEntity //object id - factory id,  settings id - custom config id
    {
        public string typeId;
        public string configId;
        public Vec2 position;
    }
}