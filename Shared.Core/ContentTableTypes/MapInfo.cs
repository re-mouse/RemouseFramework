using Remouse.Shared.Math;

namespace Remouse.Shared.ContentTableTypes
{
    public class MapInfo
    {
        public Chunk[] chunks;
    }

    public class Chunk
    {
        public int tileX;
        public int tileY;

        public string terrainId;
        public MapEntity[] mapEntities;
    }

    public class MapEntity //object id - factory id,  settings id - custom config id
    {
        public string factoryId;
        public Vec2 position;
    }
}