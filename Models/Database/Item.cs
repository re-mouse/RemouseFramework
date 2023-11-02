using System;
using Remouse.DatabaseLib;

namespace Remouse.Models.Database
{
    [Serializable]
    public class Item : TableData
    {
        public string name;
    }
}