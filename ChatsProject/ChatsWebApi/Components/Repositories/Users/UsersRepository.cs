using ClassLibrary;
using ChatsWebApi.Components.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatsWebApi.Components.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDBContext _dbContext;

        public UsersRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> CreateAsync(User item)
        {
            User? userWithSameNickName = await _dbContext.Users.FirstOrDefaultAsync(u => u.NickName == item.NickName);
            if (userWithSameNickName != null)
                return null;

            var createdItem = await _dbContext.Users.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return createdItem.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await GetByExpressionAsync(u => u.Id == id);
        }
        public async Task<User?> IsExistAsync(LoginFields loginFields)
        {
            var user = await GetByExpressionAsync(u => u.NickName == loginFields.NickName && u.Password == loginFields.Password);
            return user;
        }

        public async Task<bool> UpdateAsync(User item)
        {
            if (item == null)
                return false;

            _dbContext.Users.Update(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetByExpressionAsync(Expression<Func<User, bool>> expression)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(expression);
        }

        public async Task SetCollectionAsync<TProperty>(User item, Expression<Func<User, IEnumerable<TProperty>>> expression) where TProperty : class
        {
            await _dbContext.Entry(item).Collection(expression).LoadAsync();
        }

        public async Task SetReferenceAsync<TProperty>(User item, Expression<Func<User, TProperty>> expression) where TProperty : class
        {
            await _dbContext.Entry(item).Reference(expression).LoadAsync();
        }
    }
}
