namespace Remouse.Core.World
{
    public interface ISystem
    {
    }
    
    public interface IRunSystem : ISystem
    {
        public void Run(IWorld world);
    }
    
    public interface IPostRunSystem : ISystem
    {
        public void PostRun(IWorld world);
    }
    
    public interface IInitSystem : ISystem
    {
        public void Init(IWorld world);
    }
    
    public interface IPostInitSystem : ISystem
    {
        public void PostInit(IWorld world);
    }
}