namespace ChatsWebApi.Components.Types.Database
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string NickName { get; set; }
        public required string Password { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
