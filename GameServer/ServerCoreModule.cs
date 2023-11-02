using Remouse.GameServer.Players;
using Remouse.DatabaseLib;
using Remouse.Core;
using Remouse.Core.World;
using Remouse.DIContainer;
using Remouse.GameServer.ServerTransport;

namespace Remouse.GameServer
{
    public class ServerCoreModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<SimulationHost>();
            typeBinder.AddSingleton<ServerBootstrap>();
            typeBinder.AddSingleton<WorldCommandBuffer>();
            typeBinder.AddSingleton<PlayersSessionManager>();
            typeBinder.AddSingleton<ServerGameLoop>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<DatabaseModule>();
            moduleBinder.RegisterModule<PlayerMiddlewareModule>();
            moduleBinder.RegisterModule<SimulationModule>();
            moduleBinder.RegisterModule<WorldModule>();
        }
    }
}