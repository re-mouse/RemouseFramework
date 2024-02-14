using System;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class DefaultTextFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return true; 
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            GUILayout.TextField($"Not supported type {type.Name}");
            return value;
        }
    }
}