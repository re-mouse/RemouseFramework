using Shared.GameSimulation;

namespace GameServer.ServerShards
{
    public class GameSimulationHolder
    {
        public GameSimulation simulation { get; private set; }

        public void SetGameSimulation(GameSimulation gameSimulation)
        {
            simulation = gameSimulation;
        }
    }
}