using System;
using Remouse.Math;
using Remouse.UnityEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class Vector2IntFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(Vec2Int)
                   || type == typeof(Vector2Int);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            Vector2Int vector = type == typeof(Vector2Int) ? (Vector2Int)value : ((Vec2Int)value).ToVector2Int();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newVector = EditorGUILayout.Vector2IntField(GUIContent.none, vector);
            GUILayout.EndHorizontal();
            if (newVector != vector)
            {
                value = type == typeof(Vector2Int) ? newVector : newVector.ToVec2Int();
                isChanged = true;
            }
            return value;
        }
    }
}