using ClassLibrary;
using ChatsWebApi.Components.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly AppDBContext _dbContext;

        public ChatsRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Chat?> CreateAsync(Chat item)
        {
            var chatWithSameName = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Name.ToLower() == item.Name.ToLower());
            if (chatWithSameName != null)
                return null;

            var createdItem = await _dbContext.Chats.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return createdItem.Entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Chat? chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null)
                return false;

            _dbContext.Chats.Remove(chat);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            return await _dbContext.Chats.ToListAsync();
        }

        public async Task<Chat?> GetByIdAsync(int id)
        {
            return await GetByExpressionAsync(c => c.Id == id);
        }

        public async Task<List<Chat>> GetChatsByUserIdAsync(int userId)
        {
            return await _dbContext.Chats.Where(c => c.Users.Select(u => u.Id).Contains(userId)).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Chat item)
        {
            if (item == null)
                return false;

            _dbContext.Chats.Update(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Chat?> GetByExpressionAsync(Expression<Func<Chat, bool>> expression)
        {
            return await _dbContext.Chats.FirstOrDefaultAsync(expression);
        }

        public async Task SetCollectionAsync<TProperty>(Chat item, Expression<Func<Chat, IEnumerable<TProperty>>> expression) where TProperty : class
        {
            await _dbContext.Entry(item).Collection(expression).LoadAsync();
        }

        public async Task SetReferenceAsync<TProperty>(Chat item, Expression<Func<Chat, TProperty>> expression) where TProperty : class
        {
            await _dbContext.Entry(item).Reference(expression).LoadAsync();
        }
    }
}
