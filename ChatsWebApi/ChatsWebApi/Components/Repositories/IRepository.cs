namespace ChatsWebApi.Components.Repositories
{
    public interface IRepository<T>
    {
        public Task<bool> CreateAsync(T item);
        public Task<bool> DeleteAsync(int id);
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
    }
}
