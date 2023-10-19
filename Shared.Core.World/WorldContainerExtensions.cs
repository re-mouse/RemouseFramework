using Remouse.Shared.DIContainer;

namespace Remouse.Shared.Core.World
{
    public static class WorldContainerExtensions
    {
        public static void InstallWorld(this ContainerBuilder containerBuilder)
        {
            containerBuilder.Bind<IWorldBuilder>().WithInterfaces().As<LeoEcsWorldBuilder>().AlwaysNew();
        }
    }
}