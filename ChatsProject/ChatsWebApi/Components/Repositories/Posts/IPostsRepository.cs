using ClassLibrary;

namespace ChatsWebApi.Components.Repositories.Posts
{
    public interface IPostsRepository : IRepository<Post>
    {
        public Task<List<Post>> GetPostsByChatIdAsync(int chatId);
    }
}
