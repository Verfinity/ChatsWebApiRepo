using System.ComponentModel.DataAnnotations.Schema;

namespace ClassLibrary
{
    [Table(name: "ChatsUsers")]
    public class ChatsUsers
    {
        public required int ChatId { get; set; }
        public virtual Chat? Chat { get; set; }
        public required int UserId { get; set; }
        public virtual User? User { get; set; }
    }
}
