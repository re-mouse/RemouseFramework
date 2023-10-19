using System;
using System.Collections.Generic;

namespace Remouse.Shared.DIContainer
{
    public class ContainerBuilder
    {
        private List<BindingMetadata> _bindingTokens = new List<BindingMetadata>();
        private List<FactoryToken> _factoryTokens = new List<FactoryToken>();
        
        public BindingBuilder<T> Pack<T>() where T : class
        {
            var valueType = typeof(T);
            
            var token = RegisterToken(valueType, valueType);

            return new BindingBuilder<T>(token);
        }
        
        public BindingBuilder<T> PackInstance<T>(T t) where T : class
        {
            var valueType = typeof(T);
            
            var token = RegisterToken(valueType, valueType);
            token.Instance = t;
            
            return new BindingBuilder<T>(token);
        }
        
        public void BindFactory<T, TA>() where TA : class 
                                        where T : class, IFactory<TA>
        {
            var factoryType = typeof(T);
            var producingType = typeof(TA);
            
            RegisterFactory(factoryType, producingType);
        }

        public Container Build()
        {
            var container = new Container(_bindingTokens, _factoryTokens);
            return container;
        }
        
        private BindingMetadata RegisterToken(Type valueType, Type interfaceType)
        {
            var binding = new BindingMetadata();
            binding.BoundType = valueType;
            binding.InterfaceTypes.Add(interfaceType);
            
            _bindingTokens.Add(binding);
            
            return binding;
        }
        
        private FactoryToken RegisterFactory(Type factoryType, Type producingType)
        {
            var token = new FactoryToken();
            token.factoryType = factoryType;
            token.producingType = producingType;
            
            _factoryTokens.Add(token);
            
            return token;
        }
    }

    internal class FactoryToken
    {
        public Type factoryType;
        public Type producingType;
    }
}