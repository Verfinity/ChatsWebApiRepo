using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ClassLibrary
{
    [Table(name: "ChatsUsers")]
    public class ChatsUsers
    {
        public required int ChatId { get; set; }
        [JsonIgnore]
        public virtual Chat? Chat { get; set; }

        public required int UserId { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
