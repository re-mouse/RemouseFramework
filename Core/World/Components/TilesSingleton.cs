using Remouse.Core.World;
using Remouse.Mathlib;

namespace Remouse.Core.Components
{
    public struct TilesSingleton : IComponent
    {
        public Tile[][] tiles;
    }

    public struct Tile
    {
        public bool walkable;
    }
}