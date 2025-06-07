using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table(name: "Posts")]
    public class Post
    {
        public int Id { get; set; }
        public required string Content { get; set; }
        public required int ChatId { get; set; }
        public virtual Chat? Chat { get; set; }
        public required int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
