using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatsWebApi.Components.Types.Database
{
    [Table(name: "Chats")]
    public class Chat
    {
        [Key]
        public int Id { get; set; }
        public required string Name { get; set; }
        
        public List<User> Users { get; set; } = new List<User>();
        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
