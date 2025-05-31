using ClassLibrary;

namespace ChatsWebApi.Components.Repositories.Users
{
    public interface IUsersRepository : IRepository<User>
    {
        public Task<User?> IsExistAsync(string NickName, string Password);
        public Task<User?> GetByRefreshTokenAsync(string refreshToken);
        public Task<bool> SetRefreshTokenByIdAsync(string refreshToken, int id);
    }
}
