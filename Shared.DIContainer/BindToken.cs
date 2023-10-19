using System;
using System.Collections.Generic;

namespace Remouse.Shared.DIContainer
{
    internal class BindToken
    {
        public bool isAlwaysNew;
        public HashSet<Type> interfaceTypes = new HashSet<Type>();
        public Type valueType;
        public object value;
        public bool isLazy = true;
        public bool isDisposable;
    }
    
    public class PublicBindToken<T>
    {
        private readonly BindToken _token;

        internal PublicBindToken(BindToken token)
        {
            _token = token; 
        }
        
        public PublicBindToken<T> As<TA>() where TA : T
        {
            _token.valueType = typeof(TA);
            return this;
        }

        public PublicBindToken<T> AlwaysNew()
        {
            _token.isAlwaysNew = true;
            return this;
        }
        
        public PublicBindToken<T> WithInterfaces()
        {
            var interfaces = _token.valueType.GetInterfaces();
            
            foreach (var i in interfaces)
            {
                _token.interfaceTypes.Add(i);
            }
            
            return this;
        }
        
        public PublicBindToken<T> OnlyInterfaces()
        {
            _token.interfaceTypes.Remove(_token.valueType);
            return this;
        }

        public PublicBindToken<T> NonLazy()
        {
            _token.isLazy = false;
            return this;
        }

        public PublicBindToken<T> AsDisposable()
        {
            if (!typeof(IDisposable).IsAssignableFrom(_token.valueType))
                throw new InvalidCastException($"Type of {_token.valueType} is not IDisposable");
            
            _token.isDisposable = true;
            return this;
        }

        public PublicBindToken<T> FromInstance<TA>(TA instance) where TA : T
        {
            if (instance == null)
                throw new NullReferenceException("Instance is null");

            _token.value = instance;
            _token.valueType = typeof(TA);
            _token.isAlwaysNew = false;
            return this;
        }
    }
}