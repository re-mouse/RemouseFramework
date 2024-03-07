using Remouse.World;
using ReDI;
using Remouse.Simulation;
using Remouse.UnityEngine.Utils;
using Shared.EcsLib.LeoEcsLite;
using UnityEngine;

namespace Remouse.UnityEngine.SimulationRender
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LoadableEntityRender))]
    public abstract class EntityRender : MonoBehaviour
    {
        public abstract void UpdateRender(EcsWorld world, int entityId);
    }
}