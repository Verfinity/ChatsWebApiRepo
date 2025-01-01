﻿namespace ChatsWebApi.Components.Types
{
    public class Post
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public required int ChatId { get; set; }
        public required int UserId { get; set; }
    }
}
