using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public static class GUIColorConsts
    {
        public static Color Avaiable = new Color(0.3f, 0.93f, 0.33f, 1);
        public static Color Selected = new Color(0.7f, 0.42f, 0.97f, 1);
        public static Color Foldout = new Color(0.95f, 0.9f, 1f, 1);
        public static Color Tool = new Color(0.1f, 0.64f, 0.58f, 1);
        public static Color Error = new Color(1f, 0.5f, 0.5f, 1);
        public static Color Default = new Color(0.85f, 0.85f, 0.85f, 1);
        public static Color DefaultBackground = new Color(0.35f, 0.35f, 0.35f, 1);

        public static GUILayoutBackgroundToken StartBackground(this Color color)
        {
            return new GUILayoutBackgroundToken(color);
        } 
        
        public static GUILayoutColorToken StartColor(this Color color)
        {
            return new GUILayoutColorToken(color);
        } 
        
        public static Texture2D AsTexture(this Color color)
        {
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        } 
    }
}