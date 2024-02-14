using System;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class BoolFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type) { return type == typeof(bool); }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            bool boolValue = (bool)value;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            boolValue = EditorGUILayout.Toggle(boolValue);
            GUILayout.EndHorizontal();
            if ((bool)value != boolValue)
                isChanged = true;
            return boolValue;
        }
    }
}