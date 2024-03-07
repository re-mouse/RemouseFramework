using ReDI;

namespace Remouse.Database
{
    public class DatabaseModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.AddSingleton<IDatabase, DatabaseProxy>();
            typeBinder.AddSingleton<DatabaseBuilder>();
            typeBinder.AddSingleton<DatabaseJsonSerializer>().ImplementingInterfaces();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
            
        }
    }
}