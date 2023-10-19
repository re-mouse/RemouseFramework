namespace Remouse.Shared.DIContainer
{
    public interface IFactory<T> where T : class
    {
        public T Create();
    }
}