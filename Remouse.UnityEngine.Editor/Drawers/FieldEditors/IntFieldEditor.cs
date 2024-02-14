using System;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class IntFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(int) || type == typeof(uint)
                                       || type == typeof(ushort) || type == typeof(short);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            int intValue = (int)value;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            intValue = EditorGUILayout.IntField(intValue, GUILayout.Width(250));
            GUILayout.EndHorizontal();
            if ((int)value != intValue)
                isChanged = true;
            return intValue;
        }
    }
}