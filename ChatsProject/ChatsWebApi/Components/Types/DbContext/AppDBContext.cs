using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace ChatsWebApi.Components.Types
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ChatsUsers> ChatsUsers { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(u => u.Id);
            modelBuilder.Entity<Post>().HasKey(p => p.Id);
            modelBuilder.Entity<Chat>().HasKey(c => c.Id);

            modelBuilder
                .Entity<Chat>()
                .HasMany(u => u.Users)
                .WithMany(c => c.Chats)
                .UsingEntity<ChatsUsers>(
                    cu =>
                    cu.HasOne(cu => cu.User)
                    .WithMany()
                    .HasForeignKey(cu => cu.UserId),
                    
                    cu =>
                    cu.HasOne(cu => cu.Chat)
                    .WithMany()
                    .HasForeignKey(cu => cu.ChatId));

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
