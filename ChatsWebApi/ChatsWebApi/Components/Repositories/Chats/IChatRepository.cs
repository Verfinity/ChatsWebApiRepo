using ChatsWebApi.Components.Types;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public interface IChatRepository : IRepository<Chat>
    {
        public Task<List<Chat>> GetChatsByUserIdAsync(int userId);
    }
}
