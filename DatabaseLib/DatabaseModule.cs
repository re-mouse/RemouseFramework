using Remouse.DIContainer;

namespace Remouse.DatabaseLib
{
    public class DatabaseModule : Module
    {
        public override void BindDependencies(TypeManager typeBinder)
        {
            typeBinder.RegisterType<DatabaseHost>().AsSelf();
            typeBinder.RegisterType<DatabaseBuilder>().WithTransientLifetime();
        }

        public override void BindModuleDependencies(ModuleManager moduleBinder)
        {
        }
    }
}