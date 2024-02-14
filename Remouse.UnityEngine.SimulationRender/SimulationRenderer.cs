using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Remouse.DI;
using Remouse.Simulation;
using Remouse.Utils.ObjectPool;
using Remouse.World;
using UnityEngine;

namespace Remouse.UnityEngine.SimulationRender
{
    internal class SimulationRenderer : ISimulationRenderer
    {
        private class EntityRenderBind
        {
            public IPackedEntity packedEntity;
            public EntityRender entityRender;
            public PooledObject<GameObject> pooledObject;
        }

        private readonly Dictionary<int, EntityRenderBind> _entityRenderBinds = new Dictionary<int, EntityRenderBind>();
        private readonly Dictionary<string, PoolReserveToken> _entityPools = new Dictionary<string, PoolReserveToken>();
        private readonly Dictionary<string, AsyncLazy> _poolLoadingTasks = new Dictionary<string, AsyncLazy>();
        
        private IGameObjectsPoolManager _poolManager;
        private ISimulationHost _simulationHost;
        private ISimulation _simulation;
        private CancellationTokenSource _simulationCts;

        public void Construct(Container container)
        {
            _simulationHost = container.Resolve<ISimulationHost>();
            _poolManager= container.Resolve<IGameObjectsPoolManager>();

            _simulation = _simulationHost.Simulation;
            _simulationHost.SimulationChanged += HandleSimulationChanged;
        }

        public void Render()
        {
            HandleExistingEntities(_simulation.World);
            HandleNewEntities(_simulation.World, _simulationCts.Token);
        }

        public async UniTask PrepareToRenderEntity(string typeId)
        {
            var key = EntityRenderAssetKeys.GetKey(typeId);
            _poolLoadingTasks[key] = LoadPool(key).ToAsyncLazy();
            await _poolLoadingTasks[key];
        }
        
        public void Clear()
        {
            ClearRenders();

            foreach (var reservePoolToken in _entityPools)
            {
                reservePoolToken.Value.Release();
            }
            
            _entityPools.Clear();
        }
        
        private void HandleSimulationChanged(ISimulation simulation)
        {
            if (_simulation != null)
            {
                _simulationCts.Cancel();
                ClearRenders();
            }

            _simulation = simulation;
            
            if (_simulation != null)
            {
                _simulationCts = new CancellationTokenSource();
            }
            else
            {
                Clear();
            }
        }

        private void HandleExistingEntities(IReadOnlyWorld world)
        {
            var toRemove = new List<int>();

            foreach (var entityRenderBind in _entityRenderBinds)
            {
                var entityId = entityRenderBind.Key;
                var entity = entityRenderBind.Value.packedEntity;

                if (entity.IsAlive())
                {
                    UpdateRender(world, entityId);
                }
                else
                {
                    toRemove.Add(entityId);
                }
            }

            foreach (var entityId in toRemove)
            {
                DeleteRender(entityId);
                _entityRenderBinds.Remove(entityId);
            }
        }

        private void UpdateRender(IReadOnlyWorld world, int entityId)
        {
            if (_entityRenderBinds.TryGetValue(entityId, out var renderInfo))
            {
                renderInfo.entityRender?.UpdateRender(world, entityId);
            }
        }

        private void DeleteRender(int entityId)
        {
            if (_entityRenderBinds.TryGetValue(entityId, out var renderInfo))
            {
                renderInfo.pooledObject.Return();
                _entityRenderBinds.Remove(entityId);
            }
        }
        
        private void HandleNewEntities(IReadOnlyWorld world, CancellationToken cancellationToken)
        {
            var filter = world.Query().Inc<Render>().Inc<TypeInfo>().Inc<ReTransform>().End();
            foreach (var entityId in filter)
            {
                if (_entityRenderBinds.ContainsKey(entityId)) //render already exist
                    continue; 
                
                var entity = new EntityRenderBind();
                
                entity.packedEntity = world.PackEntity(entityId);;
                _entityRenderBinds[entityId] = entity;
                
                LoadAndCreateRenderAsync(world, entityId, cancellationToken).Forget();
            }
        }

        private async UniTask LoadAndCreateRenderAsync(IReadOnlyWorld world, int entityId, CancellationToken cancellationToken)
        {
            var typeId = world.GetComponent<TypeInfo>(entityId).typeId;

            var key = EntityRenderAssetKeys.GetKey(typeId);

            if (!_entityPools.ContainsKey(key))
            {
                if (!_poolLoadingTasks.ContainsKey(key))
                {
                    _poolLoadingTasks[key] = LoadPool(key).ToAsyncLazy();
                }

                await _poolLoadingTasks[key];
            }
            
            CreateRender(entityId, cancellationToken, key);
        }

        private void CreateRender(int entityId, CancellationToken cancellationToken, string key)
        {
            var renderBind = _entityRenderBinds[entityId];

            if (!renderBind.packedEntity.IsAlive() || cancellationToken.IsCancellationRequested)
            {
                return;
            }

            var gameObject = _entityPools[key].Pool.Get();
            renderBind.entityRender = gameObject.Value.GetComponent<EntityRender>();
            renderBind.pooledObject = gameObject;
            _entityRenderBinds[entityId] = renderBind;
        }

        private async UniTask LoadPool(string key)
        {
            _entityPools[key] = await _poolManager.GetPoolAsync(key);
        }

        private void ClearRenders()
        {
            foreach (var entityRender in _entityRenderBinds)
            {
                entityRender.Value.pooledObject.Return();
            }

            _entityRenderBinds.Clear();
        }
    }
}
