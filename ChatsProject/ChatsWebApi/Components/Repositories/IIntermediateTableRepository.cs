namespace ChatsWebApi.Components.Repositories
{
    public interface IIntermediateTableRepository<T>
    {
        public Task<T?> AddAsync(T obj);
        public Task<bool> RemoveAsync(T obj);
    }
}
