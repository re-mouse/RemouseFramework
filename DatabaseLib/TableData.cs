using System;

namespace Remouse.DatabaseLib
{
    [Serializable]
    public abstract class TableData
    {
        public string id;
        public string Id { get => id; set => id = value; }
    }
}