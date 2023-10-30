using System;
using System.Collections.Generic;

namespace Remouse.Core.Configs
{
    [Serializable]
    public class ComponentConfig
    {
        public string name;
        public Dictionary<string, string> componentValues;
    }
}