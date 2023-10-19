using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Database
{
    [Serializable]
    public class Table<T> where T : TableData
    {
        public List<T> rows = new List<T>();
        
        public T GetData(string id)
        {
            return rows.FirstOrDefault(t => t.Id == id);
        }
        
        public void ValidateRows()
        {
            HashSet<string> ids = new HashSet<string>();
            foreach (var row in rows)
            {
                if (row == null)
                    throw new NullReferenceException($"One of row in table {GetType()} is null");

                if (string.IsNullOrEmpty(row.Id))
                    throw new NullReferenceException($"Id of row in table {GetType()} is null");

                if (!ids.Add(row.Id))
                    throw new AggregateException($"Duplicate id found in table {GetType()}, rowId: {row.Id}");
            }
        }
    }
}