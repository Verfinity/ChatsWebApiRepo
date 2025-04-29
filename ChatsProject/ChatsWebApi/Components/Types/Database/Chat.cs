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
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;

        [JsonIgnore]
        public virtual List<User> Users { get; set; } = new List<User>();
        [JsonIgnore]
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
