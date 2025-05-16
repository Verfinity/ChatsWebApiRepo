namespace ChatsFrontend.Logic.SavingData
{
    public interface ISavingDataProvider<T> where T : notnull
    {
        public Task SaveDataAsync(T dataObject);
        public Task<T> GetDataAsync();
    }
}
