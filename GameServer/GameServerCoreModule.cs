using Remouse.GameServer.Players;
using Remouse.GameServer.ServerShards;
using Remouse.DatabaseLib;
using Remouse.Core;
using Remouse.Core.World;
using Remouse.DIContainer;

namespace Remouse.GameServer
{
    public class GameServerCoreModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<SimulationHost>();
            typeBinder.AddSingleton<WorldCommandBuffer>();
            typeBinder.AddSingleton<PlayersSessionManager>();
            typeBinder.AddSingleton<ServerGameLoop>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<DatabaseModule>();
            moduleBinder.RegisterModule<SimulationModule>();
            moduleBinder.RegisterModule<WorldModule>();
        }
    }
}