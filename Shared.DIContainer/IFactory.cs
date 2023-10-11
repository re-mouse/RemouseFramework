namespace Shared.DIContainer
{
    public interface IFactory<T> where T : class
    {
        public T Create();
    }
}