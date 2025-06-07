using ClassLibrary;

namespace ChatsWebApi.Components.Repositories.Users
{
    public interface IUsersRepository : IRepository<User>
    {
        public Task<User?> IsExistAsync(LoginFields loginFields);
    }
}
