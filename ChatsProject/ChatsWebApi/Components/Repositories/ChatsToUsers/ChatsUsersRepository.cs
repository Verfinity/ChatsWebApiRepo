using ClassLibrary;
using ChatsWebApi.Components.Types;

namespace ChatsWebApi.Components.Repositories.ChatsToUsers
{
    public class ChatsUsersRepository : IChatsUsersRepository
    {
        private readonly AppDBContext _dbContext;

        public ChatsUsersRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatsUsers?> AddAsync(ChatsUsers chatsUsers)
        {
            var sameChatsUsers = _dbContext.ChatsUsers.FirstOrDefault(cu => cu.ChatId == chatsUsers.ChatId && cu.UserId == chatsUsers.UserId);
            if (sameChatsUsers != null)
                return null;

            try
            {
                var createdChatsUsers = await _dbContext.ChatsUsers.AddAsync(chatsUsers);
                await _dbContext.SaveChangesAsync();
                return createdChatsUsers.Entity;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> RemoveAsync(ChatsUsers chatsUsers)
        {
            var sameChatsUsers = _dbContext.ChatsUsers.FirstOrDefault(cu => cu.ChatId == chatsUsers.ChatId && cu.UserId == chatsUsers.UserId);
            if (sameChatsUsers == null)
                return false;

            try
            {
                _dbContext.ChatsUsers.Remove(sameChatsUsers);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
