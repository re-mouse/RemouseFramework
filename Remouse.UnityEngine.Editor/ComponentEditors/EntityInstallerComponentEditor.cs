using System;
using System.Collections.Generic;
using System.Linq;
using Remouse.Simulation;
using Models.Database;
using Remouse.UnityEngine.SimulationRender;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Remouse.Utils;
using Remouse.World;

namespace Remouse.UnityEngine.Editor
{
    [CustomEditor(typeof(EntityInstallerComponent))]
    public class EntityInstallerComponentEditor : UnityEditor.Editor
    {
        private static Type[] componentTypes = TypeUtils<IComponent>.DerivedInstanceTypes;
        private static Type[] hideInSelectTypes = new Type[] { typeof(ReTransform), typeof(TypeInfo), typeof(Render) };
        
        private EntityInstallerComponent component { get => (EntityInstallerComponent)target; }

        private FieldEditorDrawer _fieldEditorDrawer = new FieldEditorDrawer();
        private SaveAsPrefabOrAddressableDrawer _saveDrawer;
        private LoadableAssetTool _loadableAssetTool;
        
        private string _selectedComponentName;
        
        public override VisualElement CreateInspectorGUI()
        {
            _loadableAssetTool = new LoadableAssetTool(component.gameObject);

            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Space(10);
            
            DrawTypeId();

            GUILayout.Space(20);

            if (_loadableAssetTool.IsEditingPrefab())
            {
                DrawComponentEditor();
                GUILayout.Space(20);
                DrawRenderInfo();
            }
            else
            {
                GUILayout.Label("Open prefab to edit entity");
            }
        }

        private void DrawRenderInfo()
        {
            if (component.GetComponent<LoadableEntityRender>() != null)
            {
                GUILayout.Label("Entity will be rendered in scene");

                if (!component.componentConfigs.Exists(c => c.componentType == nameof(Render)))
                {
                    component.componentConfigs.Add(new ComponentConfig() {componentType = nameof(Render)});
                }
                
                DrawComponentTypeFields(component.componentConfigs.FirstOrDefault(c => c.componentType == nameof(Render)), typeof(Render));
            }
            else
            {
                GUILayout.Label("Entity will not be rendered in scene");
                component.componentConfigs.RemoveAll(c => c.componentType == nameof(Render));
                
                if (GUILayout.Button("Add Default Renderer"))
                {
                    component.gameObject.AddComponent<LoadableEntityRender>();
                    EditorUtility.SetDirty(component);
                }
            }
        }

        private void DrawTypeId()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("TypeId", GUILayout.Width(50));
            var oldTypeId = component.typeId;
            component.typeId = GUILayout.TextField(component.typeId);
            if (component.typeId != oldTypeId)
            {
                EditorUtility.SetDirty(component);
            }
            GUILayout.EndHorizontal();
        }
        
        private void DrawComponentSelectButtons(List<ComponentConfig> configs)
        {
            var viewWidth = (int)EditorGUIUtility.currentViewWidth;
            var componentWidth = 135;
            var componentHeight = 25;
            var maxComponentPerRow = viewWidth / componentWidth;
            componentWidth = viewWidth / maxComponentPerRow;

            var componentTypeNames = componentTypes.Select(c => c.Name);
            var componentsOnEntityName = configs.Select(x => x.componentType);
            var options = componentTypeNames.Union(componentsOnEntityName).Except(hideInSelectTypes.Select(t => t.Name));
            
            var currentComponentsInRow = 0;
            var horizontalActive = false;
            foreach (var componentOption in options)
            {
                if (currentComponentsInRow == 0 && !horizontalActive)
                {
                    GUILayout.BeginHorizontal();
                    horizontalActive = true;
                }

                DrawComponentSelection(componentOption);

                currentComponentsInRow++;
                if (currentComponentsInRow >= maxComponentPerRow)
                {
                    currentComponentsInRow = 0;
                    GUILayout.EndHorizontal();
                    horizontalActive = false;
                }
            }
            if (horizontalActive)
            {
                GUILayout.EndHorizontal();
            }

            void DrawComponentSelection(string componentName)
            {
                var backgroundColor = GUIColorConsts.Default;

                if (_selectedComponentName == componentName)
                {
                    backgroundColor = GUIColorConsts.Selected;
                }
                else if (componentsOnEntityName.Contains(componentName) && componentTypeNames.Contains(componentName))
                {
                    backgroundColor = GUIColorConsts.Avaiable;
                }
                else if (componentsOnEntityName.Contains(componentName) && !componentTypeNames.Contains(componentName))
                {
                    backgroundColor = GUIColorConsts.Error;
                }

                var background = backgroundColor.StartBackground();

                if (GUILayout.Button(componentName, GUILayout.Width(componentWidth), GUILayout.Height(componentHeight)))
                {
                    if (_selectedComponentName != componentName)
                    {
                        _selectedComponentName = componentName;
                    }
                    else
                    {
                        _selectedComponentName = "";
                    }
                }

                background.End();
            }
        }

        private void DrawComponentEditor()
        {
            var configs = _loadableAssetTool.GetPrefab().GetComponent<EntityInstallerComponent>().componentConfigs;
            DrawComponentSelectButtons(configs);

            DrawComponentConfigEditor(configs);
        }

        private void DrawComponentConfigEditor(List<ComponentConfig> configs)
        {
            if (string.IsNullOrEmpty(_selectedComponentName))
                return;
            
            var selectedNameRef = _selectedComponentName; //linq can't accept ref variables
            var componentConfig = configs.FirstOrDefault(c => c.componentType == selectedNameRef);
            
            GUILayout.BeginVertical();
            
            if (componentConfig == null)
            {
                GUILayout.Space(10);
                DrawAddComponentButton(configs);
                GUILayout.EndVertical();
                return;
            }
            
            GUILayout.Space(10);
            
            DrawComponentTypeFields(componentConfig);

            GUILayout.Space(10);

            DrawRemoveComponentButton(configs, componentConfig);
            
            GUILayout.EndVertical();
        }

        private void DrawAddComponentButton(List<ComponentConfig> configs)
        {
            if (GUILayout.Button($"Add {_selectedComponentName} on Entity"))
            {
                var componentConfig = new ComponentConfig();
                componentConfig.componentType = _selectedComponentName;
                configs.Add(componentConfig);
                EditorUtility.SetDirty(this.component);
            }
        }

        private void DrawComponentTypeFields(ComponentConfig componentConfig)
        {
            var componentType = componentTypes.FirstOrDefault(c => c.Name == _selectedComponentName);
            if (componentType == null)
            {
                LLogger.Current.LogError(this, $"Component with name {_selectedComponentName} not found. Deleted");
                component.componentConfigs.Remove(componentConfig);
                EditorUtility.SetDirty(this.component);
                return;
            }
            
            DrawComponentTypeFields(componentConfig, componentType);
        }

        private void DrawComponentTypeFields(ComponentConfig componentConfig, Type componentType)
        {
            var horizontalActive = false;
            
            var component = ComponentFactory.CreateFromConfig(componentConfig);
            var fields = componentType.GetFields();
            
            GUILayout.BeginVertical();
            bool isAnyFieldChanged = false;
            foreach (var field in fields)
            {
                var value = _fieldEditorDrawer.DrawFieldEditor(field.FieldType, field.Name, field.GetValue(component), out bool isChanged);

                if (isChanged)
                {
                    var fieldValuePair = componentConfig.fieldValues.FirstOrDefault(c => c.fieldName == field.Name);
                    if (fieldValuePair == null)
                    {
                        fieldValuePair = new ComponentFieldValue(field.Name, value.ToString());
                        componentConfig.fieldValues.Add(fieldValuePair);
                    }
                    else
                    {
                        fieldValuePair.value = value.ToString();
                    }
                    
                    isAnyFieldChanged = true;
                }
            }
            
            if (isAnyFieldChanged)
                EditorUtility.SetDirty(this.component);
            
            GUILayout.EndVertical();
        }
        
        private void DrawRemoveComponentButton(List<ComponentConfig> configs, ComponentConfig component)
        {
            if (GUILayout.Button($"Remove {component?.componentType} from Entity"))
            {
                if (component == null)
                    configs.RemoveAll(c => c == null);
                configs.Remove(component);
                EditorUtility.SetDirty(this.component);
            }
        }
    }
}