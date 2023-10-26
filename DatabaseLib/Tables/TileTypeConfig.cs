namespace Remouse.DatabaseLib.Tables
{
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