using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using System.Data.SqlClient;
using Dapper;

namespace ChatsWebApi.Components.Repositories
{
    public class ChatsRepository : IRepository<Chat>
    {
        private readonly string _connStr;

        public ChatsRepository(DBSettings dbSettings)
        {
            _connStr = dbSettings.ConnectionString;
        }

        public async Task<bool> CreateAsync(Chat item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("INSERT INTO Chats(FirstUserId, SecondUserId) VALUES(@FirstUserId, @SecondUserId);", item);
                return result > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.ExecuteAsync("DELETE FROM Chats WHERE Id = @Id;", new { Id = id });
                return result > 0;
            }
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<Chat> chats = (await conn.QueryAsync<Chat>("SELECT * FROM Chats;")).ToList();
                return chats;
            }
        }

        public async Task<Chat?> GetByIdAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                Chat? chat = await conn.QueryFirstOrDefaultAsync<Chat>("SELECT * FROM Chats WHERE Id = @Id;", new { Id = id });
                return chat;
            }
        }
    }
}
