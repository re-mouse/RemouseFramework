using GameServer.ServerShards;
using Shared.Content;
using Shared.ContentTableTypes;
using Shared.DIContainer;
using Shared.Online.Models;
using Shared.Utils.Log;

namespace GameServer.Players
{
    public class PlayerSpawner
    {
        private GameSimulationHolder _simulationHolder;
        private PlayerSettings _playerSettings;
        
        private void Construct(Container container)
        {
            container.Get(out _simulationHolder);
            _playerSettings = container.Get<Database>().GetSettings<PlayerSettings>();
        }

        public void SpawnPlayer(PlayerData playerData)
        {
            if (playerData.sessionData.state == PlayerState.Spawned)
            {
                Logger.Current.LogPlayerError(this, playerData, "Trying to spawn player twice. Player already spawned");
                return;
            }
            
            
        }
    }
}