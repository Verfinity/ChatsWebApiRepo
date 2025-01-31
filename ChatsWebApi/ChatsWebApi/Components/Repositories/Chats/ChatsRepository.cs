using ChatsWebApi.Components.Types.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;

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
            foreach (int id in item.UsersId)
            {
                User? user = (await _dbContext.Users.ToListAsync()).FirstOrDefault(u => u.Id == id);
                if (user == null)
                    return null;

                item.Users.Add(user);
            }

            int deletedUsersCount = item.Users.Where(u => u.IsDeleted).Count();
            if (item.Users.Count - deletedUsersCount < 2)
                return null;

            List<Chat> chatsWithSameName = (await _dbContext.Chats.ToListAsync()).Where(c => c.Name == item.Name).ToList();
            foreach (Chat chat in chatsWithSameName)
            {
                if (chat.Users == item.Users)
                    return null;
            }

            await _dbContext.Chats.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
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
            return await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
