using System;
using Remouse.Core.Configs;
using Remouse.DatabaseLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class EntityTypeConfig : TableData
    {
        public ComponentConfig[] components;
    }
}