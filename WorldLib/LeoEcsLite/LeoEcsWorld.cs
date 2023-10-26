using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Remouse.Shared.EcsLib.LeoEcsLite;

namespace Remouse.Core.World
{
    internal class LeoEcsWorld : IWorld
    {
        private readonly EcsWorld _world;
        private readonly Type[] _componentTypes;
        private readonly HashSet<ISystem> _systems = new HashSet<ISystem>();
        private readonly HashSet<IRunSystem> _runSystems = new HashSet<IRunSystem>();
        private readonly HashSet<IPostRunSystem> _postRunSystems = new HashSet<IPostRunSystem>();

        public LeoEcsWorld(EcsWorld world, HashSet<ISystem> systems, HashSet<Type> componentTypes)
        {
            _world = world;

            _systems = systems;
            _componentTypes = componentTypes.ToArray();
            foreach (var system in systems)
            {
                if (system is IRunSystem runSystem)
                    _runSystems.Add(runSystem);
                
                if (system is IPostRunSystem postRunSystem)
                    _postRunSystems.Add(postRunSystem);
            }
        }

        public void Initialize()
        {
            foreach (var system in _systems)
            {
                if (system is IInitSystem initSystem)
                    initSystem.Init(this);
            }
            
            foreach (var system in _systems)
            {
                if (system is IPostInitSystem postInitSystem)
                    postInitSystem.PostInit(this);
            }
        }

        public void Tick()
        {
            foreach (var system in _runSystems)
                system.Run(this);
            
            foreach (var system in _postRunSystems)
                system.PostRun(this);
        }
        
        public int[] GetEntities()
        {
            int[]? entityArray = null;
            _world.GetAllEntities(ref entityArray);
            return entityArray;
        }

        public object? GetComponent(Type type, int entityId)
        {
            var pool = (IEcsPool)_world.GetPoolByType(type);
            
            return pool?.Has(entityId) ?? false ? pool.GetRaw(entityId) : null;
        }

        public Type[] GetComponentTypes()
        {
            return _componentTypes;
        }

        public T? GetComponent<T>(int entityId) where T : struct, IComponent
        {
            var pool = _world.GetPoolByType(typeof(T));
            return pool?.Has(entityId) ?? false ? (T)pool?.GetRaw(entityId) : null;
        }

        public ref T GetComponentRef<T>(int entityId) where T : struct, IComponent
        {
            var pool = (EcsPool<T>)_world.GetPoolByType(typeof(T));
            
            return ref pool.Get(entityId);
        }
        
        public ref T AddComponent<T>(int entityId) where T : struct, IComponent
        {
            var pool = (EcsPool<T>)_world.GetPoolByType(typeof(T));
            
            return ref pool.Add(entityId);
        }


        public void DelComponent<T>(int entityId) where T : struct, IComponent
        {
            var pool = _world.GetPoolByType(typeof(T));
            
            pool?.Del(entityId);
        }

        public void SetComponent(Type type, int entityId, object component)
        {
            var pool = _world.GetPoolByType(type);
            
            if (pool.Has(entityId))
            {
                pool?.SetRaw(entityId, component);
            }
            else
            {
                pool.AddRaw(entityId, component);
            }
        }

        public void DelComponent(Type type, int entityId)
        {
            var pool = _world.GetPoolByType(type);
            
            pool?.Del(entityId);
        }

        public bool HasComponent<T>(int entityId) where T : struct, IComponent
        {
            return HasComponent(typeof(T), entityId);
        }

        public bool HasComponent(Type type, int entityId)
        {
            var pool = _world.GetPoolByType(type);

            return pool.Has(entityId);
        }

        public IQueryBuilder Query()
        {
            return new LeoEcsQueryBuilder(_world);
        }

        public int NewEntity()
        {
            return _world.NewEntity();
        }

        public void DelEntity(int entityId)
        {
            _world.DelEntity(entityId);
        }
    }
}