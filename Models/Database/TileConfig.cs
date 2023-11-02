using System;
using Remouse.Core.Configs;
using Remouse.MathLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class TileConfig
    {
        public TableDataLink<TileTypeConfig> typeId;
        public Vec2Int position;
    }
}