using ChatsWebApi.Components.Types.Database;

namespace ChatsWebApi.Components.Repositories.Users
{
    public interface IUsersRepository : IRepository<User>
    {
        public Task<User?> IsUserExistAsync(string NickName, string Password);
        public Task<bool> SetRefreshTokenByNickNameAsync(string refreshToken, string nickName);
    }
}
