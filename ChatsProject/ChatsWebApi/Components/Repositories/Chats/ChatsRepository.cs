using ClassLibrary;
using Microsoft.EntityFrameworkCore;

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

            return (Chat?)(await createdItem.GetDatabaseValuesAsync()).ToObject();
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
            Chat? chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null )
                return null;

            return chat;
        }

        public async Task<List<Chat>> GetChatsByUserIdAsync(int userId)
        {
            return await _dbContext.Chats.Where(c => c.Users.Select(u => u.Id).Contains(userId)).ToListAsync();
        }
    }
}
