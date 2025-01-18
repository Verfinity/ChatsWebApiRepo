using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatsWebApi.Components.Types.Database
{
    [Table(name: "Posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }

        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public required Chat Chat { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public required User User { get; set; }
    }
}
