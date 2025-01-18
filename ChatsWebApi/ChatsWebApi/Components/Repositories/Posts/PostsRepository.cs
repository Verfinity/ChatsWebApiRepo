using ChatsWebApi.Components.Types.Database;
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
            if (!item.Chat.Users.Contains(item.User))
                return null;
            await _dbContext.Posts.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(Post item)
        {
            if (item == null)
                return false;

            _dbContext.Posts.Remove(item);
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
    }
}
