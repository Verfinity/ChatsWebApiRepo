using System.Linq.Expressions;

namespace ChatsWebApi.Components.Repositories
{
    public interface IRepository<T> where T : class
    {
        public Task<T?> CreateAsync(T item);
        public Task<bool> DeleteAsync(int id);
        public Task<bool> UpdateAsync(T item);
        public Task<List<T>> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);

        public Task SetCollectionAsync<TProperty>(T item, Expression<Func<T, IEnumerable<TProperty>>> expression) where TProperty : class;
        public Task SetReferenceAsync<TProperty>(T item, Expression<Func<T, TProperty>> expression) where TProperty : class;

        public Task<T?> GetByExpressionAsync(Expression<Func<T, bool>> expression);
    }
}
