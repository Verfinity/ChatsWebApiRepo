using ChatsFrontend.Logic.SavingData.TokensSaver;
using ClassLibrary;
using System.Net;
using System.Net.Http.Headers;

namespace ChatsFrontend.Logic.HttpClients
{
    public class AuthHttpClient : IAuthHttpClient
    {
        private const string RefreshPath = "api/auth/refresh";

        private readonly HttpClient _httpClient;
        private readonly ITokenPairSaver _tokenPairSaver;
        private readonly string _authScheme;

        public AuthHttpClient(HttpClient httpClient, ITokenPairSaver tokenPairSaver, string authScheme)
        {
            _httpClient = httpClient;
            _tokenPairSaver = tokenPairSaver;
            _authScheme = authScheme;
        }

        public async Task<T?> GetAsync<T>(string path)
        {
            await SetJWTToHttpClient();
            var response = await _httpClient.GetAsync(path);

            if (await CheckAuthAsync(response) == false)
                return await GetAsync<T>(path);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return await Task.FromResult<T?>(default);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<HttpResponseMessage> SendAsync<T>(string path, T obj) where T : notnull
        {
            await SetJWTToHttpClient();
            var response = await _httpClient.PostAsJsonAsync(path, obj);

            if (await CheckAuthAsync(response) == false)
                return await SendAsync<T>(path, obj);

            return response;
        }

        private async Task<bool> CheckAuthAsync(HttpResponseMessage response)
        {
            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return true;

            var currentTokenPair = await _tokenPairSaver.GetDataAsync();
            if (currentTokenPair == null)
                throw new UnauthorizedAccessException("Can't get token pair from storage!");

            var newTokenPairResponse = await _httpClient.GetAsync($"RefreshPath/{currentTokenPair.RefreshToken}");
            if (newTokenPairResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new UnauthorizedAccessException("Refresh token is invalid!");

            var newTokenPair = await newTokenPairResponse.Content.ReadFromJsonAsync<TokenPair>();
            await _tokenPairSaver.SaveDataAsync(newTokenPair);

            return false;
        }

        private async Task SetJWTToHttpClient()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization != null)
                return;

            var tokenPair = await _tokenPairSaver.GetDataAsync();
            if (tokenPair != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authScheme, tokenPair.AccessToken);
        }
    }
}
