namespace Remouse.Shared.Core.World
{
    public interface IWorld : IReadOnlyWorld
    {
        public void Tick();
        public void Initialize();
        public ref T GetComponentRef<T>(int entityId) where T : struct;
        public ref T AddComponent<T>(int entityId) where T : struct;
        public void DelComponent<T>(int entityId) where T : struct; 
        public int NewEntity();
        public void DelEntity(int entityId);
    }
}