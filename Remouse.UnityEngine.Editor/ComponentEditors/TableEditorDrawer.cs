using System.Linq;
using Remouse.Database;
using Remouse.Utils;
using UnityEditor;
using UnityEngine;

namespace Remouse.UnityEngine.Editor.EditorWindows
{
    public class TableEditorDrawer<T>  : DefaultEditorDrawer where T : TableData, new()
    {
        public override object Draw(object tableObj, out bool isChanged, params string[] excludeFieldNames)
        {
            isChanged = false;
            
            var table = tableObj as Table<T>;
            if (table == null)
            {
                var colorContext = GUIColorConsts.Error.StartBackground();
                GUILayout.Label("Inner error while parsing type");
                colorContext.End();
                return table;
            }
            
            GUILayout.BeginVertical();
            
            GUILayout.Space(8);
            
            foreach (var row in table.rows.ToList())
            {
                var oldId = row.id; 
                
                GUILayout.BeginHorizontal();
                if (GUILayout.Button($"x", GUILayout.Width(20)))
                {
                    table.rows.Remove(row);
                    isChanged = true;
                }
                
                EditorGUILayout.LabelField("Id", GUILayout.Width(15));
                bool duplicateExist = table.rows.FindAll(r => r.id == row.id).Count > 1;
                if (row.id.IsNullOrEmpty())
                {
                    var errorColor = GUIColorConsts.Error.StartBackground();
                    row.id = GUILayout.TextField(row.id);
                    GUILayout.Label("Empty", GUILayout.Width(60));
                    errorColor.End();
                }
                else if (duplicateExist)
                {
                    var errorColor = GUIColorConsts.Error.StartBackground();
                    row.id = GUILayout.TextField(row.id);
                    GUILayout.Label("Duplicate", GUILayout.Width(60));
                    errorColor.End();
                }
                else
                {
                    row.id = GUILayout.TextField(row.id);
                }
                if (oldId != row.id)
                    isChanged = true;
                GUILayout.EndHorizontal();
                
                if (isChanged)
                {
                    GUILayout.EndVertical();
                    return tableObj;
                }
                
                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                GUILayout.Space(35);
                base.Draw(row, out isChanged, "id");
                GUILayout.EndHorizontal();
                
                if (isChanged)
                    break;
                
                GUILayout.Space(10);
                GUILayoutHelper.DrawUILine(Color.gray);
            }
            
            GUILayout.Space(15);
            if (GUILayout.Button("Add table row"))
            {
                table.rows.Add(new T());
                isChanged = true;
            }
            
            GUILayout.EndVertical();

            return table;
        }
    }
}