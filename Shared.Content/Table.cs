using System;
using System.Collections.Generic;

namespace Shared.Content
{
    [Serializable]
    public class Table<T> where T : TableData
    {
        public List<T> rows = new List<T>();
        
        private Dictionary<string, T> _rowsById = new Dictionary<string, T>();

        public void Initialize()
        {
            ValidateRows();

            CreateDictionary();
        }

        public T GetData(string id)
        {
            return _rowsById.TryGetValue(id, out var data) ? data : null;
        }
        
        private void ValidateRows()
        {
            HashSet<string> ids = new HashSet<string>();
            foreach (var row in rows)
            {
                if (row == null)
                    throw new NullReferenceException($"One of row in table {GetType()} is null");

                if (string.IsNullOrEmpty(row.id))
                    throw new NullReferenceException($"Id of row in table {GetType()} is null");

                if (!ids.Add(row.id))
                    throw new AggregateException($"Duplicate id found in table {GetType()}, rowId: {row.id}");
            }
        }

        private void CreateDictionary()
        {
            foreach (var row in rows)
            {
                _rowsById[row.id] = row;
            }
        }
    }
}