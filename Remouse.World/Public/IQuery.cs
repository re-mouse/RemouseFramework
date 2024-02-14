using System;

namespace Remouse.World
{
    public interface IQuery
    {
        public IEnumerator GetEnumerator();
        
        public interface IEnumerator : IDisposable
        {
            public int Current { get; }
            public bool MoveNext();
        }
    }
}