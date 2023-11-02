using System;
using Remouse.DatabaseLib;

namespace Remouse.Core.Configs
{
    [Serializable]
    public class TableDataLink<T> where T : TableData
    {
        public string Id;
    }
    
    public static class TableLinkExtensions
    {
        public static T GetTableData<T>(this TableDataLink<T> dataLink, Database database) where T : TableData
        {
            return database.GetTableData<T>(dataLink.Id);
        }
    }
}