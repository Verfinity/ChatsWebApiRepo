using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types.Database;
using Dapper;
using System.Data.SqlClient;

namespace ChatsWebApi.Components.Repositories.Users
{
    public class UsersRepository : IRepository<User>
    {
        private readonly string _connStr;

        public UsersRepository(DBSettings dbSettings)
        {
            _connStr = dbSettings.ConnectionString;
        }

        public async Task<int?> CreateAsync(User item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<User> usersWithSameNickName = (await conn.QueryAsync<User>("SELECT * FROM Users WHERE NickName = @NickName", new { item.NickName })).ToList();
                if (usersWithSameNickName.Count > 0)
                    return null;

                int result = await conn.QuerySingleAsync<int>("INSERT INTO Users(FirstName, LastName, NickName, IsDeleted) VALUES(@FirstName, @LastName, @NickName, @IsDeleted);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int)", item);
                return result;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("UPDATE Users SET FirstName = NULL, LastName = NULL, NickName = NULL, IsDeleted = 1 WHERE Id = @Id", new { Id = id });
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
