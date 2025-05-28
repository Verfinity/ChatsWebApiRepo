using ChatsFrontend.Logic.SavingData.TokensSaver;
using ClassLibrary;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ChatsFrontend.Logic.HttpClients
{
    public class AuthHttpClient : IAuthHttpClient
    {
        private const string RefreshPath = "auth/refresh";
        private const string CheckAuthPath = "auth/ping";

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
            await SetJWTToHttpClientAsync();
            var response = await _httpClient.GetAsync(path);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshJWT();
                response = await _httpClient.GetAsync(path);
            }

            if (response.StatusCode == HttpStatusCode.NoContent)
                return await Task.FromResult<T?>(default);

            return await response.Content.ReadFromJsonAsync<T?>();
        }

        public async Task<HttpResponseMessage> SendAsync<T>(string path, T obj) where T : notnull
        {
            await SetJWTToHttpClientAsync();
            var response = await _httpClient.PostAsJsonAsync<T>(path, obj);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshJWT();
                response = await _httpClient.PostAsJsonAsync<T>(path, obj);
            }

            return response;
        }

        public async Task<bool> IsAuthorize()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await SetJWTToHttpClientAsync();
            var response = await _httpClient.GetAsync(CheckAuthPath);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    await RefreshJWT();
                }
                catch (UnauthorizedAccessException)
                {
                    return false;
                }
            }

            return true;
        }

        private async Task RefreshJWT()
        {
            var currentTokenPair = await _tokenPairSaver.GetDataAsync();
            if (currentTokenPair == null)
                throw new UnauthorizedAccessException("Can't get token pair from storage!");

            var newTokenPairResponse = await _httpClient.GetAsync($"{RefreshPath}/{currentTokenPair.RefreshToken}");
            if (newTokenPairResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new UnauthorizedAccessException("Refresh token is invalid!");

            var newTokenPair = await newTokenPairResponse.Content.ReadFromJsonAsync<TokenPair>();
            await _tokenPairSaver.SaveDataAsync(newTokenPair);
            await SetJWTToHttpClientAsync();
        }

        private async Task SetJWTToHttpClientAsync()
        {
            if (_httpClient.DefaultRequestHeaders.Authorization != null)
                return;

            var tokenPair = await _tokenPairSaver.GetDataAsync();
            if (tokenPair != null)
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_authScheme, tokenPair.AccessToken);
        }
    }
}
