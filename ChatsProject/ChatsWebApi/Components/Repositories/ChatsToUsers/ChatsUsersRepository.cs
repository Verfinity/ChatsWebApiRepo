using ChatsWebApi.Components.Types.Database;
using ClassLibrary;

namespace ChatsWebApi.Components.Repositories.ChatsToUsers
{
    public class ChatsUsersRepository : IChatsUsersRepository
    {
        private readonly AppDBContext _dbContext;

        public ChatsUsersRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ChatsUsers?> AddUserToChatAsync(ChatsUsers chatsUsers)
        {
            var sameChatsUsers = _dbContext.ChatsUsers.FirstOrDefault(cu => cu.ChatId == chatsUsers.ChatId && cu.UserId == chatsUsers.UserId);
            if (sameChatsUsers != null)
                return null;

            var createdChatsUsers = await _dbContext.ChatsUsers.AddAsync(chatsUsers);
            await _dbContext.SaveChangesAsync();

            return (ChatsUsers?)(await createdChatsUsers.GetDatabaseValuesAsync()).ToObject();
        }

        public async Task<bool> RemoveUserFromChatAsync(ChatsUsers chatsUsers)
        {
            var sameChatsUsers = _dbContext.ChatsUsers.FirstOrDefault(cu => cu.ChatId == chatsUsers.ChatId && cu.UserId == chatsUsers.UserId);
            if (sameChatsUsers == null)
                return false;

            _dbContext.ChatsUsers.Remove(sameChatsUsers);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
