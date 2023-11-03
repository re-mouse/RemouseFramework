using System;
using System.Collections.Generic;
using Remouse.Core.Configs;
using Remouse.DatabaseLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class EntityTypeConfig : TableData
    {
        public List<ComponentConfig> components;
    }
}