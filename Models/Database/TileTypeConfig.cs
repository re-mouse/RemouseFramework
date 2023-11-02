using System;
using Remouse.DatabaseLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class TileTypeConfig : TableData
    {
        public TileType type;
    }
    
    public enum TileType
    {
        Air,
        Walkable,
    }
}