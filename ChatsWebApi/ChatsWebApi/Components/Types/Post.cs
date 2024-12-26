namespace ChatsWebApi.Components.Types
{
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public bool IsLast { get; set; }
        public required int SenderUserId { get; set; }
        public required int RecipientUserId { get; set; }
    }
}
