namespace ChatsWebApi.Components.Types
{
    public class Chat
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int CountOfMembers { get; set; }
        public required List<int> UsersId { get; set; } // not in database
    }
}
