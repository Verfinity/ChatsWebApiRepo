using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using System.Data.SqlClient;
using Dapper;

namespace ChatsWebApi.Components.Repositories
{
    public class ChatsRepository : IChatRepository
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

        private record ChatsToUsers
        {
            public required int ChatId { get; set; }
            public required int UserId { get; set; }
        }

        public async Task<List<Chat>> GetChatsByUserIdAsync(int userId)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<ChatsToUsers> chatsToUsers = (await conn.QueryAsync<ChatsToUsers>("SELECT * FROM ChatsToUsers WHERE UserId = @UserID", new {UserId = userId})).ToList();

                List<Chat> chats = chatsToUsers.Count > 0 ? new List<Chat>() : null;
                foreach (ChatsToUsers chatToUser in chatsToUsers)
                {
                    chats.Add(await GetByIdAsync(chatToUser.ChatId));
                }

                return chats;
            }
        }
    }
}
