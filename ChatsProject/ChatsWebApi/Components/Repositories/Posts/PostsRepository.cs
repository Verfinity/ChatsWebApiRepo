using ClassLibrary;
using ChatsWebApi.Components.Types;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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

            await _dbContext.Entry(item.Chat).Collection(c => c.Users).LoadAsync();
            if (!item.Chat.Users.Contains(item.User))
                return null;

            var createdItem = await _dbContext.Posts.AddAsync(item);
            await _dbContext.SaveChangesAsync();

            return createdItem.Entity;
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
            return await GetByExpressionAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsByChatIdAsync(int chatId)
        {
            return await _dbContext.Posts
                .Where(p => p.ChatId == chatId)
                .Include(p => p.User)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> UpdateAsync(Post item)
        {
            if (item == null)
                return false;

            _dbContext.Posts.Update(item);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Post?> GetByExpressionAsync(Expression<Func<Post, bool>> expression)
        {
            return await _dbContext.Posts.FirstOrDefaultAsync(expression);
        }

        public async Task SetCollectionAsync<TProperty>(Post item, Expression<Func<Post, IEnumerable<TProperty>>> expression) where TProperty : class
        {
            await _dbContext.Entry(item).Collection(expression).LoadAsync();
        }

        public async Task SetReferenceAsync<TProperty>(Post item, Expression<Func<Post, TProperty>> expression) where TProperty : class
        {
            await _dbContext.Entry(item).Reference(expression).LoadAsync();
        }
    }
}
