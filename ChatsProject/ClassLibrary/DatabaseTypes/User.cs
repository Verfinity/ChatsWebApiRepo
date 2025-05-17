using ChatsWebApi.Components.Types.Roles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    [Table(name: "Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public required string NickName { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
        public required string RefreshToken { get; set; }

        [JsonIgnore]
        public virtual List<Post> Posts { get; set; } = new List<Post>();
        [JsonIgnore]
        public virtual List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
