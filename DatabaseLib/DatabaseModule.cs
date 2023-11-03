using Remouse.DIContainer;

namespace Remouse.DatabaseLib
{
    public class DatabaseModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<DatabaseHost>();
            typeBinder.AddTransient<DatabaseBuilder>();
            typeBinder.AddTransient<DatabaseJsonSerializer>();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}