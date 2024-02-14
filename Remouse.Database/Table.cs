using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Database
{
    public abstract class Table
    {
        public abstract Type GetDataType();
        public abstract List<TableData> GetRowsRaw();
    }
    
    [Serializable]
    public class Table<T> : Table where T : TableData
    {
        public List<T> rows = new List<T>();
        
        public T GetData(string id)
        {
            return rows.FirstOrDefault(t => t.id == id);
        }

        public override Type GetDataType()
        {
            return typeof(T);
        }

        public override List<TableData> GetRowsRaw()
        {
            return rows.Cast<TableData>().ToList();
        }
    }
}