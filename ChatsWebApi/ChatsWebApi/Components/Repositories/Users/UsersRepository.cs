using ChatsWebApi.Components.Repositories.Posts;
using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using Dapper;
using System.Data.SqlClient;

namespace ChatsWebApi.Components.Repositories.Users
{
    public class UsersRepository : IRepository<User>
    {
        private readonly string _connStr;
        private readonly IPostsRepository _postsRepo;

        public UsersRepository(DBSettings dbSettings, IPostsRepository postsRepo)
        {
            _connStr = dbSettings.ConnectionString;
            _postsRepo = postsRepo;
        }

        public async Task<int?> CreateAsync(User item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<User> usersWithSameNickName = (await conn.QueryAsync<User>("SELECT * FROM Users WHERE NickName = @NickName", new { item.NickName })).ToList();
                if (usersWithSameNickName.Count > 0)
                    return null;

                int result = await conn.QuerySingleAsync<int>("INSERT INTO Users(FirstName, LastName, NickName) VALUES(@FirstName, @LastName, @NickName);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int)", item);
                return result;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _postsRepo.SetPostsAsDeletedByUserIdAsync(id);
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("DELETE FROM Users WHERE Id = @Id;", new { Id = id });
                return result > 0;
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<User> users = (await conn.QueryAsync<User>("SELECT * FROM Users;")).ToList();
                return users;
            }
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                User? user = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id;", new { Id = id });
                return user;
            }
        }
    }
}
