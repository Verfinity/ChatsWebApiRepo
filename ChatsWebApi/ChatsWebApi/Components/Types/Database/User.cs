using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatsWebApi.Components.Types.Database
{
    [Table(name: "Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Length(2, 50)]
        public required string? FirstName { get; set; }
        [Length(2, 50)]
        public required string? LastName { get; set; }
        [Length(2, 50)]
        public required string? NickName { get; set; }
        [Length(8, 100)]
        public required string? Password { get; set; }
        public string? Role { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsDeleted { get; set; } = false;

        [JsonIgnore]
        public virtual List<Post> Posts { get; set; } = new List<Post>();
        [JsonIgnore]
        public virtual List<Chat> Chats { get; set; } = new List<Chat>();
    }
}
