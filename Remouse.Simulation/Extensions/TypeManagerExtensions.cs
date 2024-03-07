using ReDI;
using Remouse.World;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.Simulation
{
    public static class TypeManagerExtensions
    {
        public static BindingConfigurator<TSystem> AddWorldSystem<TSystem>(this TypeManager typeManager, ushort priority = 0) where TSystem : class, IEcsSystem, new()
        {
            typeManager
                .AddSingleton<WorldSystemInitializer<TSystem>>()
                .FromInstance(new WorldSystemInitializer<TSystem>(priority))
                .CreateOnContainerBuild();

            return typeManager.AddSingleton<TSystem>();
        }
        
        public static BindingConfigurator<TSystem> AddWorldSystem<TSystem>(this TypeManager typeManager, TSystem system, ushort priority = 0) where TSystem : class, IEcsSystem, new()
        {
            typeManager
                .AddSingleton<WorldSystemInitializer<TSystem>>()
                .FromInstance(new WorldSystemInitializer<TSystem>(system, priority))
                .CreateOnContainerBuild();
            
            return typeManager.AddSingleton<TSystem>().FromInstance(system);
        }
    }

    class WorldSystemInitializer<TSystem> where TSystem : class, IEcsSystem, new()
    {
        private readonly TSystem _system;
        private readonly ushort _priority;

        [Inject]
        public WorldSystemInitializer()
        {
            
        }

        public WorldSystemInitializer(ushort priority)
        {
            _priority = priority;
        }

        public WorldSystemInitializer(TSystem system, ushort priority) : this(priority)
        {
            _system = system; 
        }
        
        public void Construct(Container container)
        {
            var worldBuilder = container.Resolve<EcsSystemsBuilder>();
            if (_system != null)
            {
                worldBuilder.Add(_system, _priority);
            }
            else
            {
                var system = container.Resolve<TSystem>();
                worldBuilder.Add(system, _priority);
            }
        }
    }
}