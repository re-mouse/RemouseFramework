using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.Shared.DIContainer
{
    internal class BindingMetadata
    {
        public bool AlwaysNewInstance { get; set; }
        public HashSet<Type> InterfaceTypes { get; } = new HashSet<Type>();
        public Type BoundType { get; set; }
        public object Instance { get; set; }
        public bool IsLazyInitialized { get; set; } = true;
        public bool IsDisposable { get; set; }
    }
    
    public class BindingBuilder<T>
    {
        private readonly BindingMetadata _metadata;

        internal BindingBuilder(BindingMetadata metadata)
        {
            _metadata = metadata;
        }
        
        public BindingBuilder<T> As<TDerived>() where TDerived : T
        {
            _metadata.BoundType = typeof(TDerived);
            return this;
        }

        public BindingBuilder<T> AlwaysNew()
        {
            _metadata.AlwaysNewInstance = true;
            return this;
        }
        
        public BindingBuilder<T> WithInterfaces()
        {
            AddInterfaces(_metadata.BoundType.GetInterfaces());
            return this;
        }

        private void AddInterfaces(IEnumerable<Type> interfaces)
        {
            foreach (var i in interfaces)
            {
                _metadata.InterfaceTypes.Add(i);
            }
        }

        public BindingBuilder<T> OnlyInterfaces()
        {
            _metadata.InterfaceTypes.RemoveWhere(t => t == _metadata.BoundType);
            return this;
        }

        public BindingBuilder<T> NonLazy()
        {
            _metadata.IsLazyInitialized = false;
            return this;
        }

        public BindingBuilder<T> AsDisposable()
        {
            if (!typeof(IDisposable).IsAssignableFrom(_metadata.BoundType))
                throw new InvalidCastException($"Type of {_metadata.BoundType} is not IDisposable");
            
            _metadata.IsDisposable = true;
            return this;
        }

        public BindingBuilder<T> FromInstance<TInstance>(TInstance instance) where TInstance : T
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _metadata.Instance = instance;
            _metadata.BoundType = typeof(TInstance);
            _metadata.AlwaysNewInstance = false;
            return this;
        }
    }
}
