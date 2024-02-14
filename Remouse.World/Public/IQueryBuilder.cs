namespace Remouse.World
{
    public interface IQueryBuilder
    {
        public IQueryBuilder Inc<T>() where T : struct, IComponent;
        public IQueryBuilder Exc<T>() where T : struct, IComponent;
        public IQuery End();
    }
}