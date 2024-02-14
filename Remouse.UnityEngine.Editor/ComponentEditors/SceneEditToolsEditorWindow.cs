using System.Linq;
using Models.Database;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Remouse.UnityEngine.Editor
{
    public class SceneEditToolsEditorWindow : EditorWindow
    {
        [MenuItem("Window/ReMouse Scene Tool")]
        public static void CreateSceneEditorWindow()
        {
            EditorWindow.GetWindow<SceneEditToolsEditorWindow>("Scene Tool", true);
        }
        
        private SceneMapExporterTool _sceneMapExporterTool = new SceneMapExporterTool();
        private Scene _openedScene;
        
        private MapConfig _currentMap;


        public void OnGUI()
        {
            SaveMapButton();
        }

        public void SaveMapButton()
        { 
            GUILayout.Space(20);

            string mapId = SceneManager.GetActiveScene().name;
            GUILayout.Label($"Map ID: {mapId}", GUILayout.Width(60));
            
            if (GUILayout.Button($"Save at {mapId}"))
            {
                if (UnityDatabaseHost.Get() == null)
                {
                    Debug.LogError("Database host not found");
                    return;
                }
                
                MapConfig mapConfig = _sceneMapExporterTool.GetCurrentSceneAsConfig(mapId);
                if (mapConfig == null)
                {
                    Debug.LogError("Map config not found");
                    return;
                }

                var table = UnityDatabaseHost.GetOrCreateTable<MapConfig>();
                var existingConfig = table.rows.FirstOrDefault(c => c.id == mapId);
                if (existingConfig != null)
                {
                    table.rows.Remove(existingConfig);
                }

                table.rows.Add(mapConfig);
                
                UnityDatabaseHost.Save();
            }
        }
    }
}