﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChatsWebApi.Components.Types.Database
{
    [Table(name: "Posts")]
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public required string Content { get; set; }

        public required int ChatId { get; set; }
        [ForeignKey("ChatId")]
        [JsonIgnore]
        public Chat Chat { get; set; }

        public required int UserId { get; set; }
        [ForeignKey("UserId")]
        [JsonIgnore]
        public User User { get; set; }
    }
}
