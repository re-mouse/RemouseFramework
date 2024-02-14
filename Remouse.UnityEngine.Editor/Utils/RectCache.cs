using System.Collections.Generic;
using UnityEngine;

namespace Remouse.UnityEngine.Editor
{
    public class RectCache
    {
        private List<Rect> registeredRects = new List<Rect>();
        private int index;

        public void Register()
        {
            if (index < registeredRects.Count)
                registeredRects[index] = GUILayoutUtility.GetLastRect();
            else
                registeredRects.Add(GUILayoutUtility.GetLastRect());
            
            index++;
        }

        public Rect GetCurrent()
        {
            if (index >= registeredRects.Count)
            {
                Debug.Log($"Index too big, Registered only {registeredRects.Count}, request {index}");
                registeredRects.Add(Rect.zero);
            }
            
            return registeredRects[index];

        }

        public void FromZero()
        {
            index = 0;
        }
    }
}