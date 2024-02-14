using UnityEngine;

namespace Remouse.UnityEngine.Editor.EditorWindows
{
    public class DefaultEditorDrawer
    {
        protected RectCache _rectCache = new RectCache();
        private ObjectEditorDrawer _objectEditorDrawer;

        public DefaultEditorDrawer()
        {
            _rectCache = new RectCache();
            _objectEditorDrawer = new ObjectEditorDrawer(_rectCache);
        }

        public virtual object Draw(object obj, out bool isChanged, params string[] excludeFieldNames)
        {
            isChanged = false;
            
            if (obj == null)
            {
                var color = GUIColorConsts.Error.StartBackground();
                GUILayout.Label("Select data source to edit");
                color.End();
                return obj;
            }
            
            return _objectEditorDrawer.Draw(obj, obj.GetType().Name, obj.GetType(), out isChanged, excludeFieldNames);
        }
    }
}