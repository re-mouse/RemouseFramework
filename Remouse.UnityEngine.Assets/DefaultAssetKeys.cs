using System;

namespace Remouse.UnityEngine.Assets
{
    public class DefaultAssetKeys
    {
        public static string GetPrefix()
        {
            return "Default/";
        }
        
        public static string GetKey(string name)
        {
            return $"Default/{name}";
        }
    }
}