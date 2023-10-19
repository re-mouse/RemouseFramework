using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.DIContainer
{
    public class Container : IDisposable
    {
        private class ServiceRegistration
        {
            public bool AlwaysNewInstance;
            public object Instance;
            public Type ServiceType;
            public bool IsDisposable;
        }

        private readonly List<ServiceRegistration> _registrations = new List<ServiceRegistration>();
        private readonly Dictionary<Type, HashSet<ServiceRegistration>> _registrationsByInterface = new Dictionary<Type, HashSet<ServiceRegistration>>();
        
        private bool _isDisposed;

        internal Container(IEnumerable<BindingInfo> bindings)
        {
            foreach (var binding in bindings)
            {
                var registration = new ServiceRegistration
                {
                    ServiceType = binding.BoundType,
                    AlwaysNewInstance = binding.AlwaysNewInstance,
                    Instance = binding.Instance
                };
                
                foreach (var interfaceType in binding.AssociatedInterfaces)
                {
                    if (!_registrationsByInterface.TryGetValue(interfaceType, out var existingRegistrations))
                    {
                        existingRegistrations = new HashSet<ServiceRegistration>();
                        _registrationsByInterface[interfaceType] = existingRegistrations;
                    }
                    
                    existingRegistrations.Add(registration);
                }

                _registrations.Add(registration);
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
                if (registration.IsDisposable && registration.Instance is IDisposable disposable)
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

            if (registration.AlwaysNewInstance || registration.Instance == null)
            {
                registration.Instance = CreateInstance(registration.ServiceType);
            }

            return registration.Instance;
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
