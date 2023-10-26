using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.DIContainer
{
    public class Container : IDisposable
    {
        private class ServiceRegistration
        {
            public bool alwaysNewInstance;
            public object instance;
            public Type serviceType;
            public bool isDisposable;
        }

        private readonly List<ServiceRegistration> _registrations = new List<ServiceRegistration>();
        private readonly Dictionary<Type, HashSet<ServiceRegistration>> _registrationsByInterface = new Dictionary<Type, HashSet<ServiceRegistration>>();
        
        private bool _isDisposed;

        internal Container(IEnumerable<BindingInfo> bindings)
        {
            var toConstruct = new HashSet<Type>();
            
            foreach (var binding in bindings)
            {
                var registration = new ServiceRegistration
                {
                    serviceType = binding.boundType,
                    alwaysNewInstance = binding.alwaysNewInstance,
                    instance = binding.instance,
                    isDisposable = binding.isDisposable
                };
                
                foreach (var interfaceType in binding.associatedInterfaces)
                {
                    if (!_registrationsByInterface.TryGetValue(interfaceType, out var existingRegistrations))
                    {
                        existingRegistrations = new HashSet<ServiceRegistration>();
                        _registrationsByInterface[interfaceType] = existingRegistrations;
                    }
                    
                    existingRegistrations.Add(registration);
                }

                if (binding.constructOnBuild)
                    toConstruct.Add(binding.boundType);
                
                _registrations.Add(registration);
            }

            foreach (var constructType in toConstruct)
            {
                CreateInstance(constructType);
            }
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Container is already disposed");
            }

            foreach (var registration in _registrations)
            {
                if (registration.isDisposable && registration.instance is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }

            _isDisposed = true;
        }
        
        public T Resolve<T>() where T : class
        {
            CheckIfDisposed();
            return _registrationsByInterface.TryGetValue(typeof(T), out var registrations) 
                   ? (T)GetInstance(registrations.FirstOrDefault()) 
                   : null;
        }
        
        private void CheckIfDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Container is disposed");
            }
        }

        private object GetInstance(ServiceRegistration registration)
        {
            if (registration == null) return null;

            if (registration.alwaysNewInstance || registration.instance == null)
            {
                registration.instance = CreateInstance(registration.serviceType);
            }

            return registration.instance;
        }

        private object CreateInstance(Type type)
        {
            var constructorWithContainerParam = type.GetConstructor(new[] { typeof(Container) });
            
            if (constructorWithContainerParam != null)
            {
                return Activator.CreateInstance(type, this);
            }
            
            return Activator.CreateInstance(type);
        }
    }
}
