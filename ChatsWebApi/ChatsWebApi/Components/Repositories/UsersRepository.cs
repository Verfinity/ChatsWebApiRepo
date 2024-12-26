using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using Dapper;
using System.Data.SqlClient;

namespace ChatsWebApi.Components.Repositories
{
    public class UsersRepository : IRepository<User>
    {
        private readonly string _connStr;
        public UsersRepository(DBSettings dbSettings)
        {
            _connStr = dbSettings.ConnectionString;
        }

        public async Task<bool> CreateAsync(User item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("INSERT INTO Users(FirstName, LastName, NickName) VALUES(@FirstName, @LastName, @NickName);", item);
                return result > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("DELETE FROM Users WHERE Id = @Id;", new {Id = id});
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
                User? user = await conn.QueryFirstOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @Id;", new {Id = id});
                return user;
            }
        }
    }
}
