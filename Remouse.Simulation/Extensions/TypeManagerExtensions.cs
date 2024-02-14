using Remouse.DI;
using Remouse.World;

namespace Remouse.Simulation
{
    public static class TypeManagerExtensions
    {
        public static BindingConfigurator<TSystem> AddWorldSystem<TSystem>(this TypeManager typeManager, int priority = 0) where TSystem : class, ISystem, new()
        {
            typeManager
                .AddSingleton<WorldSystemInitializer<TSystem>>()
                .FromInstance(new WorldSystemInitializer<TSystem>(priority))
                .ConstructOnContainerBuild();

            return typeManager.AddSingleton<TSystem>();
        }
        
        public static BindingConfigurator<TSystem> AddWorldSystem<TSystem>(this TypeManager typeManager, TSystem system, int priority = 0) where TSystem : class, ISystem, new()
        {
            typeManager
                .AddSingleton<WorldSystemInitializer<TSystem>>()
                .FromInstance(new WorldSystemInitializer<TSystem>(system, priority))
                .ConstructOnContainerBuild();
            
            return typeManager.AddSingleton<TSystem>().FromInstance(system);
        }
    }

    class WorldSystemInitializer<TSystem> where TSystem : class, ISystem, new()
    {
        private readonly TSystem _system;
        private readonly int _priority;

        public WorldSystemInitializer(int priority)
        {
            _priority = priority;
        }

        public WorldSystemInitializer(TSystem system, int priority) : this(priority)
        {
            _system = system; 
        }
        
        public void Construct(Container container)
        {
            var worldBuilder = container.Resolve<IWorldBuilder>();
            if (_system != null)
            {
                worldBuilder.AddSystem(_system, _priority);
            }
            else
            {
                var system = container.Resolve<TSystem>();
                worldBuilder.AddSystem(system, _priority);
            }
        }
    }
}