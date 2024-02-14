using System;
using System.Collections.Generic;

namespace Remouse.UnityEngine.Editor
{
    public class FieldEditorDrawer
    {
        private List<IFieldEditor> editors;

        public FieldEditorDrawer()
        {
            editors = new List<IFieldEditor>
            {
                new Vector3FieldEditor(),
                new Vector2FieldEditor(),
                new Vector3IntFieldEditor(),
                new Vector2IntFieldEditor(),
                new Vector4FieldEditor(),
                new EnumFieldEditor(),
                new IntFieldEditor(),
                new AssetDrawer(),
                new BoolFieldEditor(),
                new TableDataLinkFieldEditor(),
                new StringFieldEditor(),
                new FloatFieldEditor(),
            };
        }

        public object DrawFieldEditor(Type fieldType, string label, object value, out bool isChanged)
        {
            isChanged = false;
            
            foreach (var editor in editors)
            {
                if (editor.CanHandle(fieldType))
                {
                    return editor.DrawField(value, label, fieldType, out isChanged);
                }
            }

            return value;
        }
        
        public bool CanEditField(Type fieldType)
        {
            foreach (var editor in editors)
            {
                if (editor.CanHandle(fieldType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
