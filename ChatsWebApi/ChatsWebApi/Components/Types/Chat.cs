namespace ChatsWebApi.Components.Types
{
    public class Chat
    {
        public int Id { get; set; }
        public required int FirstUserId { get; set; }
        public required int SecondUserId { get; set; }
    }
}
