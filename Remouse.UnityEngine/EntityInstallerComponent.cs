using System.Collections.Generic;
using Models.Database;
using UnityEngine;

namespace Remouse.UnityEngine.SimulationRender
{
    [DisallowMultipleComponent]
    public class EntityInstallerComponent : MonoBehaviour
    {
        public string typeId;

        public List<ComponentConfig> componentConfigs = new List<ComponentConfig>();

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(typeId))
                typeId = gameObject.name;
            
            componentConfigs.RemoveAll(c => c == null || c.fieldValues == null);
        }
    }
}