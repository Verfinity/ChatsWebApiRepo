using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatsWebApi.Components.Types.Database
{
    [Table(name: "Chats")]
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        [Length(2, 50)]
        public required string Name { get; set; }

        [NotMapped]
        public required List<int> UsersId { get; set; } = new List<int>();

        [JsonIgnore]
        public virtual List<User> Users { get; set; } = new List<User>();
        [JsonIgnore]
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
