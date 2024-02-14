using System;
using System.Collections.Generic;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.World
{
    internal class LeoEcsWorldBuilder : IWorldBuilder
    {
        private List<SystemRegistrationToken> _registeredSystems = new List<SystemRegistrationToken>();
        
        public void AddSystem<T>(int priority) where T : class, ISystem, new()
        {
            _registeredSystems.Add(new SystemRegistrationToken(() => new T(), priority));
        }
        
        public void AddSystem<T>(T t, int priority) where T : class, ISystem, new()
        {
            _registeredSystems.Add(new SystemRegistrationToken(() => t, priority));
        }

        public IWorld Build()
        {
            var systems = new List<ISystem>();
            
            _registeredSystems.Sort((s1, s2)=> s1.GetPriority() - s2.GetPriority());
            
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
        private int _priority;

        public SystemRegistrationToken(Func<ISystem> factoryMethod, int priority)
        {
            _factoryMethod = factoryMethod;
            _priority = priority;
        }
        
        public ISystem GetSystem()
        {
            return _factoryMethod();
        }

        public int GetPriority()
        {
            return _priority;
        }
    }
}