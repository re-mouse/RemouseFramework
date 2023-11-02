using System;

namespace Remouse.Core.World
{
    public interface IWorldBuilder
    {
        public void AddSystem<T>() where T : class, ISystem, new();
        public IWorld Build();
    }
}