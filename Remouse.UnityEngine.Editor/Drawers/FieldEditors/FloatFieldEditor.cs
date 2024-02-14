using System;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class FloatFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type) { return type == typeof(float) || type == typeof(double); }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            float floatValue = (float)value;
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            floatValue = EditorGUILayout.FloatField(floatValue);
            GUILayout.EndHorizontal();
            if ((float)value != floatValue)
                isChanged = true;
            return floatValue;
        }
    }
}