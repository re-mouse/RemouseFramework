using Remouse.UnityEngine.Assets;
using Remouse.Utils;
using UnityEditor;

namespace Remouse.UnityEngine.Editor
{
    [InitializeOnLoad]
    public static class EditorInstaller
    {
        static EditorInstaller()
        {
            LLogger.SetLogger(new UnityLogger());
        }
    }
}