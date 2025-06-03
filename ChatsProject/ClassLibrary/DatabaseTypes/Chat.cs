using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table(name: "Chats")]
    public class Chat
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public virtual List<User> Users { get; set; } = new List<User>();
        public virtual List<Post> Posts { get; set; } = new List<Post>();
    }
}
