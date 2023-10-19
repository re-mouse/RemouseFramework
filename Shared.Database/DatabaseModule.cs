using Remouse.DIContainer;

namespace Remouse.Database
{
    public class DatabaseModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.RegisterType<DatabaseProvider>().AsSelf();
            typeBinder.RegisterType<DatabaseBuilder>().WithTransientLifetime();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}