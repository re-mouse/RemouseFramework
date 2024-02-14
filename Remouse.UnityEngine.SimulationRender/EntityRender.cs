using Remouse.World;
using Remouse.DI;
using Remouse.Simulation;
using Remouse.UnityEngine.Utils;
using UnityEngine;

namespace Remouse.UnityEngine.SimulationRender
{
    [DisallowMultipleComponent]
    public class EntityRender : MonoBehaviour
    {
        public virtual void UpdateRender(IReadOnlyWorld world, int entityId)
        {
            UpdateTransform(world, entityId);
        }

        protected void UpdateTransform(IReadOnlyWorld world, int entityId)
        {
            var reTransform = world.GetComponent<ReTransform>(entityId);

            transform.position = new Vector3(reTransform.position.x, reTransform.position.y, transform.position.z);
            var eulers = transform.rotation.eulerAngles;
            eulers.y = reTransform.rotation;
            transform.rotation = Quaternion.Euler(eulers);
        }
    }
}