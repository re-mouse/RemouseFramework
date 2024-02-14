using System;
using System.Collections.Generic;
using System.Linq;
using Remouse.Database;
using UnityEditor;
using UnityEngine;
using Remouse.Utils;

namespace Remouse.UnityEngine.Editor.EditorWindows
{
    public class DatabaseEditorWindow : EditorWindow
    {
        private static readonly Type[] SettingTypes = TypeUtils<Settings>.DerivedInstanceTypes;
        private static readonly Type[] TableDataTypes = TypeUtils<TableData>.DerivedInstanceTypes;

        private readonly DatabaseEditorTool _databaseEditorTool = new DatabaseEditorTool();

        private object _selectedToEdit;
        private List<SideBarOption> _sideBarOptions;

        private FieldEditorDrawer _fieldEditorDrawer;
        private DefaultEditorDrawer _editorDrawer = new DefaultEditorDrawer();
        private Vector2 _editorScrollPosition;
        private float _sidebarScrollPosition;

        [MenuItem("Window/Remouse Database")]
        public static void CreateWindow()
        {
            EditorWindow.CreateWindow<DatabaseEditorWindow>("Database");
        }

        private void OnEnable()
        {
            _selectedToEdit = null;
            CreateOrUpdateSideBarOptions();
        }
        
        public void OnGUI()
        {
            if (UnityDatabaseHost.Get() != null)
            {
                DrawMainEditor();
            }
            else
            {
                DrawDatabaseUnavailable();
            }
        }

        private void DrawDatabaseUnavailable()
        {
            GUILayout.Label("Database not available");

            if (GUILayout.Button("Create new"))
                UnityDatabaseHost.CreateNew();
        }

        private void DrawMainEditor()
        {
            GUILayout.BeginHorizontal();

            DrawSidebar();
            GUILayout.Space(10);
            DrawEditor(out bool isChanged);

            if (isChanged)
            {
                UnityDatabaseHost.Save();
                _databaseEditorTool.FetchConfigsAndMaps();
            }

            GUILayout.EndHorizontal();
        }

        private void DrawSidebar()
        {
            GUILayout.BeginHorizontal();
            
            _sidebarScrollPosition = GUILayout.BeginScrollView(new Vector2(0, _sidebarScrollPosition), false, false, GUILayout.MaxWidth(250)).y;
            
            foreach (var option in _sideBarOptions)
            {
                option.Draw(_selectedToEdit);
            }
            
            var backgroundColor = GUIColorConsts.Tool.StartBackground();
            if (GUILayout.Button("Fetch configs from addressables", GUILayout.Height(25)))
            {
                FetchConfigsAndSave();
            }
            backgroundColor.End();

            GUILayout.EndScrollView();
            GUILayout.EndHorizontal();
        }

        private void FetchConfigsAndSave()
        {
            Debug.Log("Start fetching");
            
            _databaseEditorTool.FetchConfigsAndMaps();
            _selectedToEdit = null;
            UnityDatabaseHost.Save();
        }

        private void DrawEditor(out bool isChanged)
        {
            _editorScrollPosition = GUILayout.BeginScrollView(_editorScrollPosition);
            _editorDrawer.Draw(_selectedToEdit, out isChanged);
            GUILayout.EndScrollView();
        }

        private void CreateOrUpdateSideBarOptions()
        {
            _sideBarOptions = new List<SideBarOption>();
            _sideBarOptions.AddRange(GetExistingTableOptions()); 
            _sideBarOptions.AddRange(GetExistingSettingsOptions());
            _sideBarOptions.AddRange(GetAvailableTableOptions());
            _sideBarOptions.AddRange(GetAvailableSettingsOptions());
        }

        private IEnumerable<SideBarOption> GetExistingTableOptions()
        {
            var existingTables = UnityDatabaseHost.Get().GetTables();
            return existingTables.Select(table => new SideBarOption(() =>
            {
                var dataType = table.GetDataType();
                _editorDrawer = Activator.CreateInstance(typeof(TableEditorDrawer<>).MakeGenericType(dataType)) as DefaultEditorDrawer;
                _selectedToEdit = table;
            }, table.GetDataType().Name, table));
        }

        private IEnumerable<SideBarOption> GetExistingSettingsOptions()
        {
            var existingSettings = UnityDatabaseHost.Get().GetSettings();
            return existingSettings.Select(settings => new SideBarOption(() =>
            {
                _editorDrawer = new DefaultEditorDrawer();
                _selectedToEdit = settings;
            }, settings.GetType().Name, settings));
        }

        private IEnumerable<SideBarOption> GetAvailableTableOptions()
        {
            var existingTables = UnityDatabaseHost.Get().GetTables();
            var availableTablesDataTypes = TableDataTypes.Except(existingTables.Select(t => t.GetDataType()));
            return availableTablesDataTypes.Select(availableTableDataType => new SideBarOption(() =>
            {
                var tableType = typeof(Table<>).MakeGenericType(availableTableDataType);
                var table = Activator.CreateInstance(tableType) as Table;
                _editorDrawer = Activator.CreateInstance(typeof(TableEditorDrawer<>).MakeGenericType(availableTableDataType)) as DefaultEditorDrawer;
                UnityDatabaseHost.AddTable(table);
                UnityDatabaseHost.Save();
                CreateOrUpdateSideBarOptions();
                _selectedToEdit = table;
            }, $"Add {availableTableDataType.Name}", availableTableDataType));
        }

        private IEnumerable<SideBarOption> GetAvailableSettingsOptions()
        {
            var existingSettings = UnityDatabaseHost.Get().GetSettings();
            var availableSettingTypes = SettingTypes.Except(existingSettings.Select(t => t.GetType()));
            return availableSettingTypes.Select(availableSettingsType => new SideBarOption(() =>
            {
                var settings = Activator.CreateInstance(availableSettingsType) as Settings;
                _editorDrawer = new DefaultEditorDrawer();
                UnityDatabaseHost.AddSettings(settings);
                UnityDatabaseHost.Save();
                CreateOrUpdateSideBarOptions();
                _selectedToEdit = settings;
            }, $"Add {availableSettingsType.Name}", availableSettingsType));
        }
    }

    public class SideBarOption
    {
        private readonly Action _onClick;
        private readonly string _optionText;
        private readonly object _option;

        public SideBarOption(Action onClick, string optionText, object option)
        {
            _onClick = onClick;
            _optionText = optionText;
            _option = option;
        }

        public object GetOptionObject()
        {
            return _option;
        }

        public void Draw(object selectedToEdit)
        {
            bool selected = _option == selectedToEdit;
            var color = selected ? GUIColorConsts.Selected.StartBackground() : GUIColorConsts.Avaiable.StartBackground();
            if (GUILayout.Button(_optionText, GUILayout.Height(25)))
            {
                _onClick();
            }
            color.End();
        }
    }
}
