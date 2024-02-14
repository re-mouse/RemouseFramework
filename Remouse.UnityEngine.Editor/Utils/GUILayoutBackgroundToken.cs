using System;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public struct GUILayoutBackgroundToken : IDisposable
    {
        private Color? _oldColor;

        public GUILayoutBackgroundToken(Color color)
        {
            _oldColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }

        public void End()
        {
            if (_oldColor != null)
            {
                GUI.backgroundColor = _oldColor.Value;
                _oldColor = null;
            }
        }

        void IDisposable.Dispose()
        {
            if (_oldColor != null)
            {
                GUI.backgroundColor = _oldColor.Value;
                _oldColor = null;
            }
        }
    }
}