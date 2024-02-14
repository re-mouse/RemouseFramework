using System;

namespace Remouse.Database
{
    [Serializable]
    public abstract class TableDataLinkBase
    {
        public string Id;
    }
    
    [Serializable]
    public class TableDataLink<T> : TableDataLinkBase where T : TableData
    {
    }
}