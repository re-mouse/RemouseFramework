using System;

namespace ReDI
{
    public class NotFoundInjectingConstructorException : Exception
    {
        private readonly Type _type;

        public NotFoundInjectingConstructorException(Type type) { _type = type; }
        
        public override string Message { get => $"Not found constructor with [Inject] attribute, multiple constructor's defined in {_type}"; }
    }
}