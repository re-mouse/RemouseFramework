using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class ObjectEditorDrawer
    {
        private readonly RectCache _rectCache;
        private FieldEditorDrawer _fieldEditorDrawer = new FieldEditorDrawer();
        public ObjectEditorDrawer(RectCache rectCache) { _rectCache = rectCache; }

        public object Draw(object obj, string label, Type type, out bool isChanged, params string[] excludeFieldNames)
        {
            isChanged = false;

            if (obj == null && type != typeof(string)) //string null allowed
            {
                return HandleNullObject(obj, label, type, ref isChanged);
            }

            if (obj is IList list)
            {
                return DrawList(list, type, label, ref isChanged);
            }

            if (_fieldEditorDrawer.CanEditField(type))
            {
                return _fieldEditorDrawer.DrawFieldEditor(type, label, obj, out isChanged);
            }

            return DrawFields(obj, out isChanged, excludeFieldNames);
        }

        private object HandleNullObject(object obj, string label, Type type, ref bool isChanged)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            var errorColor = GUIColorConsts.Error.StartBackground();
            GUILayout.Label("Null");

            if (GUILayout.Button("Try create default"))
            {
                obj = CreateDefaultObject(type);
                isChanged = true;
            }

            GUILayout.EndHorizontal();
            errorColor.End();

            return obj;
        }

        private object CreateDefaultObject(Type type)
        {
            return type.IsArray
                ? Array.CreateInstance(type.GetElementType(), 0)
                : Activator.CreateInstance(type);
        }

        private object DrawList(IList list, Type type, string label, ref bool isChanged)
        {
            GUILayout.BeginVertical();

            Type dataType = GetListDataType(list, type);

            if (dataType == null)
            {
                GUILayout.Label("Unsupported List type. Use Array/List");
                GUILayout.EndVertical();
                return list;
            }

            list = HandleListItems(list, dataType, label, ref isChanged);
            list = HandleListAddition(list, dataType, type, ref isChanged);

            GUILayout.EndVertical();

            return list;
        }

        private Type GetListDataType(IList list, Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }
            else if (type.GetGenericTypeDefinition() == typeof(List<>))
            {
                return list.GetType().GetGenericArguments().Single();
            }

            return null;
        }

        private IList HandleListItems(IList list, Type dataType, string label, ref bool isChanged)
        {
            int toRemove = -1;

            GUILayout.Label(label);
            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(10);

                if (GUILayout.Button("X", GUILayout.Width(20), GUILayout.Height(20)))
                {
                    isChanged = true;
                    toRemove = i;
                    GUILayout.EndHorizontal();
                    break;
                }

                GUILayout.BeginVertical();

                list[i] = Draw(list[i], label, dataType, out isChanged);

                GUILayout.EndVertical();
                GUILayout.EndHorizontal();

                if (isChanged)
                {
                    break;
                }
            }

            if (toRemove >= 0)
            {
                list = DeleteListItem(list, toRemove, dataType);
                isChanged = true;
            }

            return list;
        }

        private IList DeleteListItem(IList list, int index, Type dataType)
        {
            if (list.IsFixedSize && dataType.IsArray)
            {
                Array array = list as Array;
                Array newArray = Array.CreateInstance(dataType, array.Length - 1);
                int newIndex = 0;

                for (int i = 0; i < array.Length; i++)
                {
                    if (i != index)
                    {
                        newArray.SetValue(array.GetValue(i), newIndex);
                        newIndex++;
                    }
                }

                return newArray;
            }
            else if (!list.IsFixedSize)
            {
                list.RemoveAt(index);
            }

            return list;
        }

        private IList HandleListAddition(IList list, Type dataType, Type type, ref bool isChanged)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(25);
            
            if (GUILayout.Button("Add"))
            {
                var newValue = Activator.CreateInstance(dataType);

                list = AddNewItemToList(list, newValue, dataType, type);

                isChanged = true;
            }

            GUILayout.EndHorizontal();
            return list;
        }

        private IList AddNewItemToList(IList list, object newItem, Type elementType, Type type)
        {
            if (list.IsFixedSize && type.IsArray)
            {
                Array array = list as Array;
                Array newArray = Array.CreateInstance(elementType, array.Length + 1);
                Array.Copy(array, newArray, array.Length);
                newArray.SetValue(newItem, array.Length);
                return newArray;
            }
            else if (!list.IsFixedSize)
            {
                list.Add(newItem);
            }
            
            return list;
        }

        private object DrawFields(object obj, out bool isChanged, string[] excludeFieldNames)
        {
            isChanged = false;

            var type = obj.GetType();
            var fields = type.GetFields();
            GUILayout.BeginHorizontal();
            GUILayout.Space(15);
            GUILayout.BeginVertical();
            foreach (var field in fields)
            {
                if (excludeFieldNames.Contains(field.Name))
                    continue;

                DrawAsField(obj, out isChanged, field);
                if (isChanged)
                {
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal(); 
                    return obj; 
                }

                GUILayout.Space(10);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            return obj;
        }

        private void DrawAsField(object obj, out bool isChanged, FieldInfo field)
        {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            object newFieldValue = Draw(field.GetValue(obj), field.Name, field.FieldType, out isChanged);
            if (isChanged)
            {
                field.SetValue(obj, newFieldValue);
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
    }
}