﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Shared.DIContainer
{
    public class Container : IDisposable
    {
        private class ObjectToken
        {
            public bool isAlwaysNew;
            public object value;
            public Type valueType;
            public bool isDisposable;
        }
        
        private readonly List<ObjectToken> _objectTokens = new List<ObjectToken>();
        private readonly Dictionary<Type, HashSet<ObjectToken>> _objectTokensByInterface = new Dictionary<Type, HashSet<ObjectToken>>();
        
        private bool _isDisposed;

        internal Container(List<BindToken> registerTokens, List<FactoryToken> factoryTokens)
        {
            Dictionary<Type, ObjectToken> createdTokens = new Dictionary<Type, ObjectToken>();
            
            foreach (var registerToken in registerTokens)
            {
                var objectToken = new ObjectToken();
                objectToken.valueType = registerToken.valueType;
                objectToken.isAlwaysNew = registerToken.isAlwaysNew;
                objectToken.value = registerToken.value;

                RegisterTokenAtInterfaces(objectToken, registerToken.interfaceTypes);
                
                _objectTokens.Add(objectToken);
                createdTokens.Add(objectToken.valueType, objectToken);
            }

            foreach (var registerToken in registerTokens)
            {
                if (registerToken.isLazy)
                    continue;
                
                if (registerToken.isAlwaysNew)
                    continue;

                var objectToken = createdTokens[registerToken.valueType];

                objectToken.value = CreateObjectOfType(objectToken.valueType);;
            }
        }

        private void RegisterTokenAtInterfaces(ObjectToken token, HashSet<Type> interfaceTypes)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                if (_objectTokensByInterface.TryGetValue(interfaceType, out var tokens))
                {
                    tokens.Add(token);
                }
                else
                {
                    _objectTokensByInterface[interfaceType] = new HashSet<ObjectToken>();
                    _objectTokensByInterface[interfaceType].Add(token);
                }
            }
        }
        
        public void Dispose()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Already disposed");
            }

            foreach (var objectToken in _objectTokens)
            {
                if (!objectToken.isDisposable || objectToken.value == null)
                    continue;

                if (objectToken.value is IDisposable disposeObject)
                    disposeObject.Dispose();
            }

            _isDisposed = true;
        }
        
        public I Get<I>() where I : class
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Container was disposed");
            
            var objectToken = GetInterfaceObjectTokens<I>()?.FirstOrDefault();
            
            if (objectToken != null)
            {
                return (I)GetObject(objectToken);
            }

            return null;
        }
        
        public void Get<I>(out I value) where I : class
        {
            value = Get<I>();
        }
        
        public void GetAll<I>(List<I> result) where I : class
        {
            if (_isDisposed)
                throw new ObjectDisposedException("Container was disposed");
            
            var objectTokens = GetInterfaceObjectTokens<I>();

            if (objectTokens == null)
                return;
            
            foreach (var objectToken in objectTokens)
            {
                var @object = GetObject(objectToken);
                
                result.Add((I)@object);
            }
        }

        private object GetObject(ObjectToken objectToken)
        {
            if (objectToken.isAlwaysNew)
                return CreateObjectOfType(objectToken.valueType);

            if (objectToken.value == null)
                objectToken.value = CreateObjectOfType(objectToken.valueType);

            return objectToken.value;
        }

        private HashSet<ObjectToken> GetInterfaceObjectTokens<I>()
        {
            Type requiredType = typeof(I);

            if (_objectTokensByInterface.ContainsKey(requiredType))
            {
                return _objectTokensByInterface[requiredType];
            }

            return null;
        }
        
        private object CreateObjectOfType(Type type)
        {
            object value;
            
            ConstructorInfo mediatorConstructor = type.GetConstructor(new[] { typeof(Container) });
            if (mediatorConstructor != null)
            {
                value = Activator.CreateInstance(type, this);
            }
            else
            {
                value = Activator.CreateInstance(type);
            }

            MethodInfo methodWithContainer = type.GetMethod("Construct", new[] { typeof(Container) });
            MethodInfo methodWithoutMediator = type.GetMethod("Construct", Type.EmptyTypes);

            if (methodWithContainer != null)
            {
                methodWithContainer.Invoke(value, new object[] { this });
            }
            else if (methodWithoutMediator != null)
            {
                methodWithoutMediator.Invoke(value, null);
            }

            return value;
        }
    }
}