namespace ChatsWebApi.Components.Types
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string NickName { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
