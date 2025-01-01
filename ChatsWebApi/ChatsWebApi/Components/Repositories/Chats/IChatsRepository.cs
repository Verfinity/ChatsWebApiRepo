using ChatsWebApi.Components.Types;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public interface IChatsRepository : IRepository<Chat>
    {
        public Task<List<Chat>> GetChatsByUserIdAsync(int userId);
        public Task<List<int>> GetUsersIdByChatIdAsync(int chatId);
    }
}
