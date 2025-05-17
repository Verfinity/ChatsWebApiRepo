using ClassLibrary;

namespace ChatsFrontend.Logic.HttpClients
{
    public interface IAuthHttpClient
    {
        public Task<HttpResponseMessage> SendAsync<T>(string path, T obj);
        public Task<T?> GetAsync<T>(string path);
    }
}
