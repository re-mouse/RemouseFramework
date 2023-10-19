using Remouse.DatabaseLib;

namespace Remouse.Models.ContentTableTypes
{
    public class Map : TableData
    {
        public TileInfo[] tileInfos;
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
        
    }
}