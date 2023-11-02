using System;
using System.Collections.Generic;
using Remouse.Shared.EcsLib.LeoEcsLite;

namespace Remouse.Core.World
{
    internal class LeoEcsWorldBuilder : IWorldBuilder
    {
        private HashSet<SystemRegistrationToken> _registeredSystems = new HashSet<SystemRegistrationToken>();
        
        public void AddSystem<T>() where T : class, ISystem, new()
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
            
            var world = new LeoEcsWorld(ecsWorld, systems);
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
}