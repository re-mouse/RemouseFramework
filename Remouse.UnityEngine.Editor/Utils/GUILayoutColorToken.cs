using System;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public struct GUILayoutColorToken : IDisposable
    {
        private Color? _oldColor;

        public GUILayoutColorToken(Color color)
        {
            _oldColor = GUI.backgroundColor;
            GUI.color = color;
        }

        public void End()
        {
            if (_oldColor != null)
            {
                GUI.color = _oldColor.Value;
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