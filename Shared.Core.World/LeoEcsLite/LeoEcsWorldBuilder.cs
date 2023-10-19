using System;
using System.Collections.Generic;
using Remouse.Shared.EcsLib.LeoEcsLite;

namespace Remouse.Shared.Core.World
{
    internal class LeoEcsWorldBuilder : IWorldBuilder
    {
        private HashSet<SystemRegistrationToken> _registeredSystems = new HashSet<SystemRegistrationToken>();
        private HashSet<ComponentRegistrationToken> _registeredPools = new HashSet<ComponentRegistrationToken>();
        
        public void RegisterComponentType<T>() where T : struct, IComponent
        {
            _registeredPools.Add(new ComponentRegistrationToken(world => world.GetPool<T>(), typeof(T)));
        }

        public void RegisterSystem<T>() where T : class, ISystem, new()
        {
            _registeredSystems.Add(new SystemRegistrationToken(() => new T()));
        }

        public IWorld Build()
        {
            var systems = new HashSet<ISystem>();
            foreach (var systemToken in _registeredSystems)
            {
                systems.Add(systemToken.GetSystem());
            }
            
            var ecsWorld = new EcsWorld();
            var componentTypes = new HashSet<Type>();
            foreach (var componentToken in _registeredPools)
            {
                componentToken.AddPoolInWorld(ecsWorld);
                componentTypes.Add(componentToken.type);
            }
            
            var world = new LeoEcsWorld(ecsWorld, systems, componentTypes);
            return world;
        }
    }

    internal struct SystemRegistrationToken
    {
        private readonly Func<ISystem> _factoryMethod;

        public SystemRegistrationToken(Func<ISystem> factoryMethod)
        {
            _factoryMethod = factoryMethod; 
        }
        
        public ISystem GetSystem()
        {
            return _factoryMethod();
        }
    }
    
    internal struct ComponentRegistrationToken
    {
        private readonly Action<EcsWorld> _componentInstaller;
        public readonly Type type;

        public ComponentRegistrationToken(Action<EcsWorld> componentInstaller, Type componentType)
        {
            _componentInstaller = componentInstaller;
            type = componentType;
        }
        
        public void AddPoolInWorld(EcsWorld ecsWorld)
        {
            _componentInstaller(ecsWorld);
        }
    }
}