using ClassLibrary;

namespace ChatsFrontend.Logic.Authorization
{
    public interface IUserAuthorizer
    {
        public Task<bool> RegisterUserAsync(LoginFields loginFields);
        public Task<bool> LoginUserAsync(LoginFields loginFields);
        public Task SignOutUserAsync();
    }
}
