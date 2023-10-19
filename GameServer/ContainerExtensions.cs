using Remouse.GameServer.Players;
using Remouse.GameServer.ServerShards;
using Remouse.Shared.Content;
using Remouse.Shared.Core;
using Remouse.Shared.DIContainer;

namespace Remouse.GameServer
{
    public static class ContainerExtensions
    {
        public static void InstallGameServer(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Bind<SimulationFactory>();
            containerBuilder.Bind<SimulationHost>();

            containerBuilder.Bind<WorldCommandBuffer>();
            
            containerBuilder.Bind<PlayersSessionManager>().WithInterfaces();
            containerBuilder.Bind<ServerGameLoop>().WithInterfaces();
        }
        
        public static void InstallDatabaseFromJson(this ContainerBuilder containerBuilder, string jsonPath)
        {
            containerBuilder.BindInstance(DatabaseSerializer.DeserializeDatabase(jsonPath));
            
            containerBuilder.Bind<SimulationFactory>();
            containerBuilder.Bind<SimulationHost>();

            containerBuilder.Bind<WorldCommandBuffer>();
            
            containerBuilder.Bind<PlayersSessionManager>().WithInterfaces();
            containerBuilder.Bind<ServerGameLoop>().WithInterfaces();
        }
    }
}