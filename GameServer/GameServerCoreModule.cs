using Remouse.GameServer.Players;
using Remouse.GameServer.ServerShards;
using Remouse.DatabaseLib;
using Remouse.Models.ContentTableTypes;
using Remouse.Core;
using Remouse.Core.World;
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