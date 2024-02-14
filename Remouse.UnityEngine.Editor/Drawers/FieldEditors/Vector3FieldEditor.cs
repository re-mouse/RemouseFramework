using System;
using Remouse.Math;
using Remouse.UnityEngine.Utils;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class Vector3FieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type == typeof(Vec3)
                   || type == typeof(Vector3);
        }

        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            Vector3 vector = type == typeof(Vector3) ? (Vector3)value : ((Vec3)value).ToVector3();
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var newVector = EditorGUILayout.Vector3Field(GUIContent.none, vector);
            if (newVector != vector)
            {
                value = type == typeof(Vector3) ? newVector : newVector.ToVec3();
                isChanged = true;
            }
            GUILayout.EndHorizontal();
            return value;
        }
    }
}