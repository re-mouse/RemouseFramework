using System;
using Remouse.Math;
using Remouse.UnityEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class Vector2FieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(Vec2)
                   || type == typeof(Vector2);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            Vector2 vector = type == typeof(Vector2) ? (Vector2)value : ((Vec2)value).ToVector2();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newVector = EditorGUILayout.Vector2Field(GUIContent.none, vector);
            GUILayout.EndHorizontal();
            if (newVector != vector)
            {
                value = type == typeof(Vector2) ? newVector : newVector.ToVec2();
                isChanged = true;
            }
            return value;
        }
    }
}