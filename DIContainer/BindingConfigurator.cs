using System;
using System.Collections.Generic;

namespace Remouse.DIContainer
{
    internal class BindingInfo
    {
        public bool AlwaysNewInstance { get; set; }
        public HashSet<Type> AssociatedInterfaces { get; } = new HashSet<Type>();
        public Type BoundType { get; set; }
        public object Instance { get; set; }
    }
    
    public class BindingConfigurator<T>
    {
        private readonly BindingInfo _binding;

        internal BindingConfigurator(BindingInfo binding)
        {
            _binding = binding;
        }

        public BindingConfigurator<T> AsType<TImplementation>() where TImplementation : T
        {
            _binding.BoundType = typeof(TImplementation);
            return this;
        }

        public BindingConfigurator<T> WithSingletonLifetime()
        {
            _binding.AlwaysNewInstance = false;
            return this;
        }

        public BindingConfigurator<T> WithTransientLifetime()
        {
            _binding.AlwaysNewInstance = true;
            return this;
        }

        public BindingConfigurator<T> ImplementingInterfaces()
        {
            _binding.AssociatedInterfaces.UnionWith(_binding.BoundType.GetInterfaces());
            return this;
        }

        public BindingConfigurator<T> AsSelf()
        {
            _binding.AssociatedInterfaces.Add(_binding.BoundType);
            return this;
        }

        public BindingConfigurator<T> As<TInterface>() where TInterface : class
        {
            _binding.AssociatedInterfaces.Add(typeof(TInterface));
            return this;
        }
    }
}