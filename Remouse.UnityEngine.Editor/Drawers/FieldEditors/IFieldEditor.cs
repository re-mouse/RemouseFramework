using System;

namespace Remouse.UnityEngine.Editor
{
    public interface IFieldEditor
    {
        bool CanHandle(Type type);
        object DrawField(object value, string label, Type type, out bool isChanged);
    }

}