using Remouse.GameServer.Players;
using Remouse.GameServer.ServerShards;
using Remouse.Database;
using Remouse.Shared.ContentTableTypes;
using Remouse.Shared.Core;
using Remouse.Shared.Core.World;
using Remouse.DIContainer;

namespace Remouse.GameServer
{
    public class GameServerCoreModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.RegisterType<SimulationHost>().AsSelf();

            typeBinder.RegisterType<WorldCommandBuffer>().AsSelf();
            
            typeBinder.RegisterType<PlayersSessionManager>().ImplementingInterfaces();
            typeBinder.RegisterType<ServerGameLoop>().ImplementingInterfaces();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            moduleBinder.RegisterModule<DatabaseModule>();
            moduleBinder.RegisterModule<SimulationModule>();
            moduleBinder.RegisterModule<WorldModule>();
        }
    }
}