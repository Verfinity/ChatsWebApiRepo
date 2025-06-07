using ChatsWebApi.Components.Types.Roles;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table(name: "Users")]
    public class User
    {
        public int Id { get; set; }
        public required string NickName { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
        public required string RefreshToken { get; set; }
        public virtual List<Post> Posts { get; set; } = new List<Post>();
        public virtual List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
