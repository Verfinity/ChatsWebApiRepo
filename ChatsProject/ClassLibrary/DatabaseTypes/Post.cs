using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    [Table(name: "Posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }

        public required int ChatId { get; set; }
        [JsonIgnore]
        public virtual Chat? Chat { get; set; }

        public required int UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
