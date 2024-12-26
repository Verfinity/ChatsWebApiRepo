using ChatsWebApi.Components.Types;

namespace ChatsWebApi.Components.Repositories
{
    public interface IChatRepository : IRepository<Chat>
    {
        public Task<List<Chat>> GetChatsByUserIdAsync(int userId);
    }
}
