using System;

namespace Remouse.UnityEngine.UI
{
    public class UIAssetKeys
    {
        public static string GetUIViewPrefix()
        {
            return "UI/";
        }
        
        public static string GetCanvasPrefix()
        {
            return "Canvas/";
        }
        
        public static string GetUIViewKey(Type type)
        {
            return $"{GetUIViewPrefix()}{type.Name}";
        }
        
        public static string GetCanvasKey(CanvasType canvasType)
        {
            return $"{GetCanvasPrefix()}{canvasType}";
        }
    }
}