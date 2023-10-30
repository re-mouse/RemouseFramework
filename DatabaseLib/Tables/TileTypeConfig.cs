using System;

namespace Remouse.DatabaseLib.Tables
{
    [Serializable]
    public class TileTypeConfig : TableData
    {
        public string renderId;
        public TileType type;
    }
    
    public enum TileType
    {
        Air,
        Walkable,
    }
}