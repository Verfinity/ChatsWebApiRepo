﻿using ChatsWebApi.Components.Types;

namespace ChatsWebApi.Components.Repositories.Posts
{
    public interface IPostsRepository : IRepository<Post>
    {
        public Task<List<Post>> GetPostsByChatIdAsync(int chatId);
    }
}
