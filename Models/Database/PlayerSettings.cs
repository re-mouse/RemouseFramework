using System;
using Remouse.Core.Configs;
using Remouse.DatabaseLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class PlayerSettings : Settings
    {
        public TableDataLink<EntityTypeConfig> playerEntityId;
    }
}