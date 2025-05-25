namespace ChatsWebApi.Components.Repositories
{
    public interface IIntermediateTableRepository<T>
    {
        public Task<T?> AddUserToChatAsync(T obj);
        public Task<bool> RemoveUserFromChatAsync(T obj);
    }
}
