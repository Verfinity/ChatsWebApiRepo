using ChatsWebApi.Components.Types.Database;
using ClassLibrary;

namespace ChatsWebApi.Components.Repositories.Users
{
    public interface IUsersRepository : IRepository<User>
    {
        public Task<User?> IsExistAsync(string NickName, string Password);
        public Task<User?> GetByRefreshTokenAsync(string refreshToken);
        public Task<bool> SetRefreshTokenByIdAsync(string refreshToken, int id);
        public Task<bool> AddUserToChatAsync(int chatId, int userId);
        public Task<bool> RemoveUserFromChatAsync(int chatId, int userId);
    }
}
