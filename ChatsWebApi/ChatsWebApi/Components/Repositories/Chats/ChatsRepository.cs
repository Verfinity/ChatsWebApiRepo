using ChatsWebApi.Components.Settings;
using System.Data.SqlClient;
using Dapper;
using ChatsWebApi.Components.Types.Database;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly string _connStr;
        private readonly IRepository<User> _usersRepo;

        public ChatsRepository(DBSettings dbSettings, IRepository<User> usersRepo)
        {
            _connStr = dbSettings.ConnectionString;
            _usersRepo = usersRepo;
        }

        public async Task<int?> CreateAsync(Chat item)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                if (item.UsersId.Count < 2)
                    return null;

                List<User> usersInChat = new List<User>();
                foreach (int id in item.UsersId)
                {
                    User user = await _usersRepo.GetByIdAsync(id);
                    if (user.IsDeleted)
                        continue;
                    usersInChat.Add(user);
                }

                if (usersInChat.Count < 2)
                    return null;

                List<int> chatsIdWithSameName = (await conn.QueryAsync<int>("SELECT Id FROM Chats WHERE Name = @Name", item)).ToList();
                item.UsersId.Sort();
                foreach (int id in chatsIdWithSameName)
                {
                    List<int> usersId = await GetUsersIdByChatIdAsync(id);
                    usersId.Sort();

                    if (usersId.Count != item.UsersId.Count)
                        break;

                    bool isCompare = true;
                    for (int i = 0; i < usersId.Count; i++)
                    {
                        if (usersId[i] != item.UsersId[i])
                        {
                            isCompare = false;
                            break;
                        }
                    }
                    if (isCompare)
                        return null;
                }

                int result = await conn.QuerySingleAsync<int>("INSERT INTO Chats(Name, CountOfMembers) VALUES(@Name, @CountOfMembers);" +
                    "SELECT CAST(SCOPE_IDENTITY() as int)", new
                    {
                        Name = item.Name,
                        CountOfMembers = item.UsersId.Count
                    });
                foreach (int userId in item.UsersId)
                {
                    await conn.ExecuteAsync("INSERT INTO ChatsToUsers(ChatId, UserId) VALUES(@ChatId, @UserId)", new
                    {
                        ChatId = result,
                        UserId = userId
                    });
                }

                return result;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                await conn.ExecuteAsync("DELETE FROM ChatsToUsers WHERE ChatId = @Id", new { Id = id });

                int result = await conn.ExecuteAsync("DELETE FROM Chats WHERE Id = @Id;", new { Id = id });
                return result > 0;
            }
        }

        public async Task<List<Chat>> GetAllAsync()
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<Chat> chats = (await conn.QueryAsync<Chat>("SELECT * FROM Chats;")).ToList();
                for (int i = 0; i < chats.Count; i++)
                {
                    await SetUsersIdInChat(chats[i]);
                }
                return chats;
            }
        }

        public async Task<Chat?> GetByIdAsync(int id)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                Chat? chat = await conn.QueryFirstOrDefaultAsync<Chat>("SELECT * FROM Chats WHERE Id = @Id;", new { Id = id });
                await SetUsersIdInChat(chat);
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

        public async Task<List<int>> GetUsersIdByChatIdAsync(int chatId)
        {
            using (var conn = new SqlConnection(_connStr))
            {
                List<int> usersId = (await conn.QueryAsync<int>("SELECT UserId FROM ChatsToUsers WHERE ChatId = @Id", new { Id = chatId })).ToList();
                return usersId;
            }
        }

        private async Task SetUsersIdInChat(Chat chat)
        {
            chat.UsersId = await GetUsersIdByChatIdAsync(chat.Id);
        }
    }
}
