﻿using ChatsWebApi.Components.Types.Database;

namespace ChatsWebApi.Components.Repositories.Chats
{
    public interface IChatsRepository : IRepository<Chat>
    {
        public Task<List<Chat>> GetChatsByUserIdAsync(int userId);
    }
}
