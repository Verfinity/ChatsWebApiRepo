using ChatsWebApi.Components.Types.Database;
using ClassLibrary;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public interface IChatsRepository : IRepository<Chat>
    {
        public Task<List<Chat>> GetChatsByUserIdAsync(int userId);
    }
}
