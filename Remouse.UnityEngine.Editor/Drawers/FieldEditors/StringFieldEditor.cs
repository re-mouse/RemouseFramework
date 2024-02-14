using System;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class StringFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(string);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            string str = (string)value;

            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            str = EditorGUILayout.DelayedTextField(str, GUILayout.MaxWidth(250));
            GUILayout.EndHorizontal();
            if (str != (string)value)
                isChanged = true;
            return str;
        }
    }
}