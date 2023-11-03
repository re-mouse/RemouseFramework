using System;
using Remouse.DatabaseLib;

namespace Remouse.Core.Configs
{
    [Serializable]
    public abstract class TableDataLinkBase
    {
        public string Id;
    }
    
    [Serializable]
    public class TableDataLink<T> :TableDataLinkBase where T : TableData
    {
    }
    
    public static class TableLinkExtensions
    {
        public static T GetTableData<T>(this TableDataLink<T> dataLink, Database database) where T : TableData
        {
            return database.GetTableData<T>(dataLink.Id);
        }
    }
}