using System;
using System.Linq;
using Remouse.Database;
using Models.Database;
using Remouse.UnityEngine.SimulationRender;
using Remouse.Utils;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Remouse.UnityEngine.Editor
{
    public class DatabaseEditorTool
    {
        private readonly AddressableTool _addressableTool = new AddressableTool();

        public bool TryValidateTable(Table table)
        {
            try
            {
                table?.GetType().GetMethod("ValidateRows")?.Invoke(table, null);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error on getting rows from table {table}");
                Debug.LogException(e);
                return false;
            }
        }
        
        public void FetchConfigsAndMaps()
        {
            FetchEntityConfigs(); 
            UnityDatabaseHost.Save();
        }
        
        private void FetchEntityConfigs()
        {
            Table<EntityTypeConfig> entityTables = new Table<EntityTypeConfig>();
            entityTables.rows.Clear();
            var entityAddressables = _addressableTool.FindAddressablesStartingWith(EntityRenderAssetKeys.GetPrefix());

            foreach (var addressable in entityAddressables)
            {
                var gameObject = Addressables.LoadAssetAsync<GameObject>(addressable.address).WaitForCompletion();
                if (gameObject == null)
                {
                    LLogger.Current.LogError(this,
                        $"Error on loading game object with address {addressable.address}, in the group {addressable.parentGroup}");
                    continue;
                }

                var installer = gameObject.GetComponent<EntityInstallerComponent>();
                if (installer == null)
                {
                    LLogger.Current.LogError(this,
                        $"Not found {typeof(EntityInstallerComponent)} on address {addressable.address}, path = {addressable.AssetPath}");
                    continue;
                }

                var config = entityTables.rows.FirstOrDefault(t => t.id == installer.typeId);
                
                if (config == null)
                {
                    config = new EntityTypeConfig { id = installer.typeId };
                    config.components = installer.componentConfigs.ToList();
                    entityTables.rows.Add(config);
                }
                else
                {
                    LLogger.Current.LogError(this, $"Found duplicate entity with typeId {installer.typeId}");
                }
            }
            
            UnityDatabaseHost.AddOrReplaceTable(entityTables);
        }
    }
}