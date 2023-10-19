using System;
using System.Collections.Generic;

namespace Remouse.Shared.DIContainer
{
    public class ContainerBuilder
    {
        private List<BindToken> _bindingTokens = new List<BindToken>();
        private List<FactoryToken> _factoryTokens = new List<FactoryToken>();
        
        public PublicBindToken<T> Bind<T>() where T : class
        {
            var valueType = typeof(T);
            
            var token = RegisterToken(valueType, valueType);

            return new PublicBindToken<T>(token);
        }
        
        public PublicBindToken<T> BindInstance<T>(T t) where T : class
        {
            var valueType = typeof(T);
            
            var token = RegisterToken(valueType, valueType);
            token.value = t;
            
            return new PublicBindToken<T>(token);
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
        
        private BindToken RegisterToken(Type valueType, Type interfaceType)
        {
            var token = new BindToken();
            token.valueType = valueType;
            token.interfaceTypes.Add(interfaceType);
            
            _bindingTokens.Add(token);
            
            return token;
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