using System;
using Remouse.Math;
using Remouse.UnityEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class Vector3IntFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(Vec3Int)
                   || type == typeof(Vector3Int);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            Vector3Int vector = type == typeof(Vector3Int) ? (Vector3Int)value : ((Vec3Int)value).ToVector3Int();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newVector = EditorGUILayout.Vector3IntField(GUIContent.none, vector);
            GUILayout.EndHorizontal();
            if (newVector != vector)
            {
                value = type == typeof(Vector3Int) ? newVector : newVector.ToVec3Int();
                isChanged = true;
            }
            return value;
        }
    }
}