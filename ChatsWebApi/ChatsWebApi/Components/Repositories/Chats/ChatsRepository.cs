using ChatsWebApi.Components.Settings;
using ChatsWebApi.Components.Types;
using System.Data.SqlClient;
using Dapper;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public class ChatsRepository : IChatRepository
    {
        private readonly string _connStr;

        public ChatsRepository(DBSettings dbSettings)
        {
            _connStr = dbSettings.ConnectionString;
        }

        public async Task<int?> CreateAsync(Chat item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                int result = await conn.QuerySingleAsync<int>("INSERT INTO Chats(FirstUserId, SecondUserId) VALUES(@FirstUserId, @SecondUserId);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int)", item);

                await conn.ExecuteAsync("INSERT INTO ChatsToUsers(ChatId, UserId) VALUES(@ChatId, @UserId);", new { ChatId = item.Id, UserId = item.FirstUserId });
                await conn.ExecuteAsync("INSERT INTO ChatsToUsers(ChatId, UserId) VALUES(@ChatId, @UserId);", new { ChatId = item.Id, UserId = item.SecondUserId });

                return result;
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
                List<ChatsToUsers> chatsToUsers = (await conn.QueryAsync<ChatsToUsers>("SELECT * FROM ChatsToUsers WHERE UserId = @UserID", new { UserId = userId })).ToList();

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
