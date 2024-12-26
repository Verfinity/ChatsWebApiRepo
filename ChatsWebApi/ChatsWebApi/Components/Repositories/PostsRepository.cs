using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using System.Data.SqlClient;
using Dapper;

namespace ChatsWebApi.Components.Repositories
{
    public class PostsRepository : IRepository<Post>
    {
        private readonly string _connStr;

        public PostsRepository(DBSettings dbSettings)
        {
            _connStr = dbSettings.ConnectionString;
        }

        public async Task<bool> CreateAsync(Post item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("INSERT INTO Posts(Title, Content, IsLast, SenderUserId, RecipientUserId) VALUES(@Title, @Content, @IsLast, @SenderUserId, @RecipientUserId);", item);
                return result > 0;
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
    }
}
