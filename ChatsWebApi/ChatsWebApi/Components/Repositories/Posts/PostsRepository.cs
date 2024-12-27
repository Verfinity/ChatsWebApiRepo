using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using System.Data.SqlClient;
using Dapper;

namespace ChatsWebApi.Components.Repositories.Posts
{
    public class PostsRepository : IPostRepository
    {
        private readonly string _connStr;

        public PostsRepository(DBSettings dbSettings)
        {
            _connStr = dbSettings.ConnectionString;
        }

        public async Task<int?> CreateAsync(Post item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.QuerySingleAsync<int>("INSERT INTO Posts(Content, IsLast, SenderUserId, ChatId) VALUES(@Content, @IsLast, @SenderUserId, @RecipientUserId, @ChatId);" +
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
