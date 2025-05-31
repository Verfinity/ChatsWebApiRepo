using ChatsFrontend.Logic.HttpClients;
using ChatsFrontend.Logic.SavingData.TokensSaver;
using ClassLibrary;
using System.Net;

namespace ChatsFrontend.Logic.Authorization
{
    public class UserAuthorizer : IUserAuthorizer
    {
        private const string RegisterPath = "auth/register";
        private const string LoginPath = "auth/login";

        private readonly ITokenPairSaver _tokenPairSaver;
        private readonly IAuthHttpClient _authHttpClient;

        public UserAuthorizer(ITokenPairSaver tokenPairSaver, IAuthHttpClient authHttpClient)
        {
            _tokenPairSaver = tokenPairSaver;
            _authHttpClient = authHttpClient;
        }

        public async Task<bool> RegisterUserAsync(LoginFields loginFields)
        {
            return await AuthorizeUserAsync(loginFields, RegisterPath);
        }

        public async Task<bool> LoginUserAsync(LoginFields loginFields)
        {
            return await AuthorizeUserAsync(loginFields, LoginPath);
        }

        public async Task SignOutUserAsync()
        {
            await _tokenPairSaver.DeleteDataAsync();

        }

        private async Task<bool> AuthorizeUserAsync(LoginFields loginFields, string authPath)
        {
            var response = await _authHttpClient.SendAsync(authPath, loginFields);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var tokenPair = await response.Content.ReadFromJsonAsync<TokenPair>();
                await _tokenPairSaver.SaveDataAsync(tokenPair);
                return true;
            }
            return false;
        }
    }
}
