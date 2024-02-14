namespace Remouse.Simulation
{
    public interface ISimulationLoader
    {
        void LoadEmpty(long startTick = 0);
        void LoadMap(string mapId, long startTick = 0);
        void LoadSerialized(WorldState packedWorldState, long startTick = 0);
    }
}