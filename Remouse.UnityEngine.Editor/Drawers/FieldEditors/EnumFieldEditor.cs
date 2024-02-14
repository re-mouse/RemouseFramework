using System;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class EnumFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type.IsEnum;
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;

            if (!Enum.TryParse(type, value.ToString(), out object enumValue))
            {
                isChanged = true;
                return Enum.GetValues(type).GetValue(0); //fallback
            }
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newEnum = EditorGUILayout.EnumPopup((Enum)enumValue, GUILayout.MaxWidth(250));
            GUILayout.EndHorizontal();
            
            if (!Equals(newEnum, enumValue))
                isChanged = true;
            
            return newEnum;
        }
    }
}