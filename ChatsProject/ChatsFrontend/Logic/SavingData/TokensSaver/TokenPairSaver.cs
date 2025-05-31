using ClassLibrary;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ChatsFrontend.Logic.SavingData.TokensSaver
{
    public class TokenPairSaver : ITokenPairSaver
    {
        private readonly string _key;
        private readonly ProtectedBrowserStorage _storage;

        public TokenPairSaver(string key, ProtectedBrowserStorage storage)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));
            if (storage == null)
                throw new ArgumentNullException(nameof(storage));

            _key = key;
            _storage = storage;
        }

        public async Task<bool> DeleteDataAsync()
        {
            try
            {
                await _storage.DeleteAsync(_key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<TokenPair?> GetDataAsync()
        {
            try
            {
                var result = await _storage.GetAsync<TokenPair>(_key);
                return result.Value;
            }
            catch
            {
                return null;
            }
        }

        public async Task SaveDataAsync(TokenPair dataObject)
        {
            if (dataObject == null)
                throw new NullReferenceException(nameof(dataObject));

            await _storage.SetAsync(_key, dataObject);
        }
    }
}
