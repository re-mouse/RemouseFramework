using System;
using System.Collections.Generic;
using System.Linq;

namespace Remouse.DIContainer
{
    public class ModuleManager
    {
        private readonly Dictionary<Type, Module> _modules = new Dictionary<Type, Module>();

        public void RegisterModule<TModule>() where TModule : Module, new()
        {
            var moduleType = typeof(TModule);
            if (!_modules.ContainsKey(moduleType))
            {
                _modules[moduleType] = new TModule();
            }
        }

        internal IEnumerable<Module> GetRegisteredModules() => _modules.Values;
    }

    public class TypeManager
    {
        private readonly List<BindingInfo> _bindings = new List<BindingInfo>();
        
        public BindingConfigurator<T> RegisterType<T>() where T : class
        {
            var binding = new BindingInfo
            {
                BoundType = typeof(T)
            };
            
            _bindings.Add(binding);
            
            return new BindingConfigurator<T>(binding);
        }
        
        internal IEnumerable<BindingInfo> GetBindings() => _bindings.AsReadOnly();
    }
}