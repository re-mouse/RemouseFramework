namespace Remouse.Core.World
{
    public interface IQuery
    {
        public IEnumerator GetEnumerator();
        
        public interface IEnumerator
        {
            public int Current { get; }
            public bool MoveNext();
        }
    }
}