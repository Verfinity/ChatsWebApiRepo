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
            item.Users = (await _dbContext.Users.ToListAsync()).Where(u => item.UsersId.Contains(u.Id)).ToList();

            User? deletedUser = item.Users.FirstOrDefault(u => u.IsDeleted == true);
            if (deletedUser != null || item.Users.Count() < 2)
                return null;

            List<Chat> chatsWithSameName = (await _dbContext.Chats.ToListAsync()).Where(c => c.Name == item.Name).ToList();
            foreach (Chat chat in chatsWithSameName)
            {
                if (item.Users.Count() != chat.Users.Count())
                    continue;

                bool isCompare = true;
                for (int i = 0; i < item.Users.Count(); i++)
                {
                    if (item.Users[i] != chat.Users[i])
                    {
                        isCompare = false;
                        break;
                    }
                }

                if (isCompare)
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
            Chat? chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == id);
            if (chat == null )
                return null;

            chat.UsersId = chat.Users.Select(u => u.Id).ToList();
            return chat;
        }
    }
}
