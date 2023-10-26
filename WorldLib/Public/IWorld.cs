using System;

namespace Remouse.Core.World
{
    public interface IWorld : IReadOnlyWorld
    {
        public void Initialize();
        
        public void Tick();
        
        public ref T GetComponentRef<T>(int entityId) where T : struct, IComponent;
        
        public ref T AddComponent<T>(int entityId) where T : struct, IComponent;
        public void DelComponent<T>(int entityId) where T : struct, IComponent;

        public void SetComponent(Type type, int entityId, object component);
        public void DelComponent(Type type, int entityId); 
        
        public int NewEntity();
        public void DelEntity(int entityId);
    }
}