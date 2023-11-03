using System;
using Remouse.DatabaseLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class MapConfig : TableData
    {
        public TileConfig[] tiles;
        public EntityConfig[] entities;
        public string test;
    }
}