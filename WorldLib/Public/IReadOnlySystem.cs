namespace Remouse.Core.World
{
    public interface IInitReadOnlySystem : IReadOnlySystem
    {
        public void Init(IReadOnlyWorld world);
    }
    
    public interface IPostInitReadOnlySystem : IReadOnlySystem
    {
        public void PostInit(IReadOnlyWorld world);
    }
    
    public interface IPostRunReadOnlySystem : IReadOnlySystem
    {
        public void PostRun(IReadOnlyWorld world);
    }
    
    public interface IRunReadOnlySystem : IReadOnlySystem
    {
        public void Run(IReadOnlyWorld world);
    }

    public interface IReadOnlySystem
    {
    }
}