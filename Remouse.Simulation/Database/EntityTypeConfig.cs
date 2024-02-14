using System;
using System.Collections.Generic;
using Remouse.Database;

namespace Models.Database
{
    [Serializable]
    public class EntityTypeConfig : TableData
    {
        public List<ComponentConfig> components;
    }
    
    [Serializable]
    public class ComponentConfig
    {
        public string componentType = "";
        public List<ComponentFieldValue> fieldValues = new List<ComponentFieldValue>();
    }
    
    [Serializable]
    public class ComponentFieldValue
    {
        public string fieldName = "";
        public string value = "";

        public ComponentFieldValue(string fieldName, string value)
        {
            this.fieldName = fieldName;
            this.value = value;
        }

        public ComponentFieldValue()
        {
            
        }
    }
}