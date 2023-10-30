using System;
using Remouse.Core.Configs;

namespace Remouse.DatabaseLib.Tables
{
    [Serializable]
    public class EntityTypeConfig : TableData
    {
        public ComponentConfig[] components;
    }
}