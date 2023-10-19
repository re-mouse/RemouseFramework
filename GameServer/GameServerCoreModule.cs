using Remouse.GameServer.Players;
using Remouse.GameServer.ServerShards;
using Remouse.Shared.Content;
using Remouse.Shared.ContentTableTypes;
using Remouse.Shared.Core;
using Remouse.Shared.DIContainer;

namespace Remouse.GameServer
{
    public class GameServerCoreModule
    {
        public void InstallGameServer(ContainerBuilder containerBuilder)
        {
            var databaseBuilder = new DatabaseBuilder();
            databaseBuilder.BindSettings(new PlayerSettings() {playerEntityId = "Player"});
            
            containerBuilder.PackInstance(databaseBuilder.Build());
            
            containerBuilder.Pack<SimulationFactory>();
            containerBuilder.Pack<SimulationHost>();

            containerBuilder.Pack<WorldCommandBuffer>();
            
            containerBuilder.Pack<PlayersSessionManager>().WithInterfaces();
            containerBuilder.Pack<ServerGameLoop>().WithInterfaces();
        }
        
        public InstallDatabaseFromJson(ContainerBuilder containerBuilder, string jsonPath)
        {
            containerBuilder.PackInstance(DatabaseSerializer.DeserializeDatabase(jsonPath));
            
            containerBuilder.Pack<SimulationFactory>();
            containerBuilder.Pack<SimulationHost>();

            containerBuilder.Pack<WorldCommandBuffer>();
            
            containerBuilder.Pack<PlayersSessionManager>().WithInterfaces();
            containerBuilder.Pack<ServerGameLoop>().WithInterfaces();
        }
    }
}