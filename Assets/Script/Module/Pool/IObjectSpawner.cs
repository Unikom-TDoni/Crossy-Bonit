namespace Lncodes.Module.Unity.Pool
{
    public interface IObjectSpawner<T> where T : class
    {
        T Create();

        void OnTake(T pooledObj);

        void OnReturned(T pooledObj);

        void Destroy(T pooledObj);
    }
}