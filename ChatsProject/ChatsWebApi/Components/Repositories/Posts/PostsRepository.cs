using ClassLibrary;
using Microsoft.EntityFrameworkCore;

namespace ChatsWebApi.Components.Repositories.Posts
{
    public class PostsRepository : IPostsRepository
    {
        private readonly AppDBContext _dbContext;

        public PostsRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Post?> CreateAsync(Post item)
        {
            item.User = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == item.UserId);
            item.Chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == item.ChatId);
            if (item.User == null || item.Chat == null)
                return null;

            if (!item.Chat.Users.Contains(item.User))
                return null;

            var createdItem = await _dbContext.Posts.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return (Post?)(await createdItem.GetDatabaseValuesAsync()).ToObject();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            Post? post = await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
            if (post == null)
                return false;

            _dbContext.Posts.Remove(post);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Post>> GetAllAsync()
        {
            return await _dbContext.Posts.ToListAsync();
        }

        public async Task<Post?> GetByIdAsync(int id)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsByChatIdAsync(int chatId)
        {
            return await _dbContext.Posts.Where(p => p.ChatId == chatId).ToListAsync();
        }
    }
}
