using Microsoft.EntityFrameworkCore;

namespace ChatsWebApi.Components.Types.Database
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Post> Posts { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Chat>()
                .HasMany(u => u.Users)
                .WithMany(c => c.Chats);

            modelBuilder
                .Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId);

            modelBuilder
                .Entity<Post>()
                .HasOne(p => p.Chat)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.ChatId);
        }
    }
}
