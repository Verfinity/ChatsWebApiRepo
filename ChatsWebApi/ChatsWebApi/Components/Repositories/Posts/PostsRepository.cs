using ChatsWebApi.Components.Settings;
using System.Data.SqlClient;
using Dapper;
using ChatsWebApi.Components.Repositories.Chats;
using ChatsWebApi.Components.Types.Database;

namespace ChatsWebApi.Components.Repositories.Posts
{
    public class PostsRepository : IPostsRepository
    {
        private readonly string _connStr;
        private readonly IChatsRepository _chatsRepo;

        public PostsRepository(DBSettings dbSettings, IChatsRepository chatsRepo)
        {
            _connStr = dbSettings.ConnectionString;
            _chatsRepo = chatsRepo;
        }

        public async Task<int?> CreateAsync(Post item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<int> usersId = await _chatsRepo.GetUsersIdByChatIdAsync(item.ChatId);
                if (!usersId.Contains(item.UserId))
                    return null;

                int result = await conn.QuerySingleAsync<int>("INSERT INTO Posts(Content, ChatId, UserId) VALUES(@Content, @ChatId, @UserId);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int)", item);
                return result;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("DELETE FROM Posts WHERE Id = @Id;", new { Id = id });
                return result > 0;
            }
        }

        public async Task<List<Post>> GetAllAsync()
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<Post> posts = (await conn.QueryAsync<Post>("SELECT * FROM Posts;")).ToList();
                return posts;
            }
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                Post? post = await conn.QueryFirstOrDefaultAsync<Post>("SELECT * FROM Posts WHERE Id = @Id;", new { Id = id });
                return post;
            }
        }

        public async Task<List<Post>> GetPostsByChatIdAsync(int chatId)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<Post> posts = (await conn.QueryAsync<Post>("SELECT * FROM Posts WHERE ChatId = @ChatId", new { ChatId = chatId })).ToList();
                return posts;
            }
        }
    }
}
