using System;
using System.Collections.Generic;
using Shared.EcsLib.LeoEcsLite;

namespace Remouse.World
{
    public class EcsSystemsBuilder
    {
        private List<SystemRegistrationToken> _registeredSystems = new List<SystemRegistrationToken>();
        
        public void Add<T>(int priority) where T : class, IEcsSystem, new()
        {
            _registeredSystems.Add(new SystemRegistrationToken(() => new T(), priority));
        }
        
        public void Add<T>(T t, int priority) where T : class, IEcsSystem, new()
        {
            _registeredSystems.Add(new SystemRegistrationToken(() => t, priority));
        }

        public EcsSystems Build(EcsWorld world)
        {
            _registeredSystems.Sort((s1, s2)=> s1.GetPriority() - s2.GetPriority());
            
            var ecsWorld = new EcsWorld();
            var systems = new EcsSystems(ecsWorld);
            foreach (var systemToken in _registeredSystems)
            {
                systems.Add(systemToken.GetSystem());
            }

            return systems;
        }
    }

    internal struct SystemRegistrationToken
    {
        private readonly Func<IEcsSystem> _factoryMethod;
        private int _priority;

        public SystemRegistrationToken(Func<IEcsSystem> factoryMethod, int priority)
        {
            _factoryMethod = factoryMethod;
            _priority = priority;
        }
        
        public IEcsSystem GetSystem()
        {
            return _factoryMethod();
        }

        public int GetPriority()
        {
            return _priority;
        }
    }
}