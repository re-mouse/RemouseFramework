using System;
using Remouse.Math;
using Remouse.UnityEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class Vector4FieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(Vec4)
                   || type == typeof(Vector4);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            Vector4 vector = type == typeof(Vector4) ? (Vector4)value : ((Vec4)value).ToVector4();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newVector = EditorGUILayout.Vector4Field(GUIContent.none, vector);
            if (newVector != vector)
            {
                value = type == typeof(Vector4) ? newVector : newVector.ToVec4();
                isChanged = true;
            }
            GUILayout.EndHorizontal();
            return value;
        }
    }
}