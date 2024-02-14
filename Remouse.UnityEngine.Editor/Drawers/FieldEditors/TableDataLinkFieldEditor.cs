using System;
using System.Linq;
using Remouse.Database;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class TableDataLinkFieldEditor : IFieldEditor
    {
        public bool CanHandle(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(TableDataLink<>);
        }
        
        public object DrawField(object value, string label, Type type, out bool isChanged)
        {
            isChanged = false;
            var tableLink = value as TableDataLinkBase;

            var table = GetTable(type);
            if (table == null)
            {
                AskCreateNewTable(type);
                return value;
            }

            var linkId = tableLink.Id;

            GUILayout.BeginHorizontal();
            GUILayout.Label(label, GUILayout.Width(FieldEditorConsts.FieldWidth));
            if (IsTableEmpty(table))
            {
                var errorColor = GUIColorConsts.Error.StartBackground();
                GUILayout.Label("Table has no data to select", GUILayout.Width(250));
                errorColor.End();
            }
            else if (IsLinkedRowExist(linkId, table))
            {
                tableLink.Id = ShowRowsSelectionPopup(linkId, table);
            }
            else
            {
                var errorColor = GUIColorConsts.Error.StartBackground();
                tableLink.Id = ShowRowsSelectionPopup(linkId, table);
                errorColor.End();
            }
            GUILayout.EndHorizontal();
            
            if (tableLink.Id != linkId)
                isChanged = true;

            return value;
        }

        private string ShowRowsSelectionPopup(string selected, Table table)
        {
            var rowsOptions = table.GetRowsRaw().Select(r => r.id).ToArray();
            int selectedIndex = table.GetRowsRaw().FindIndex(r => r.id == selected);
            selectedIndex = selectedIndex < 0 ? 0 : selectedIndex;
            
            int newSelectedIndex = EditorGUILayout.Popup(selectedIndex, rowsOptions, GUILayout.Width(250));
            return rowsOptions[newSelectedIndex];
        }

        private bool IsLinkedRowExist(string linkId, Table table)
        {
            return table.GetRowsRaw().Exists(r => r.id == linkId);
        }

        private bool IsTableEmpty(Table table)
        {
            return table.GetRowsRaw().Count == 0;
        }

        private static Table GetTable(Type type)
        {
            var dataType = type.GetGenericArguments().Single();
            return UnityDatabaseHost.GetTable(dataType);
        }

        private static void AskCreateNewTable(Type type)
        {
            var dataType = type.GetGenericArguments().Single();
            
            GUILayout.BeginHorizontal();
            var errorColor = GUIColorConsts.Error.StartBackground();
            
            GUILayout.Label($"Table of type {dataType.Name} not created");
            
            if (GUILayout.Button("Create"))
            {
                var tableType = typeof(Table<>).MakeGenericType(dataType);
                var table = Activator.CreateInstance(tableType) as Table;
                UnityDatabaseHost.AddTable(table);
            }

            errorColor.End();
            GUILayout.EndHorizontal();
        }
    }
}