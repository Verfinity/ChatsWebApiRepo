using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatsWebApi.Components.Types.Database
{
    [Table(name: "Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string? FirstName { get; set; }
        public required string? LastName { get; set; }
        public required string? NickName { get; set; }
        public required string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsDeleted { get; set; } = false;

        public List<Post> Posts { get; set; } = new List<Post>();
        public List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
