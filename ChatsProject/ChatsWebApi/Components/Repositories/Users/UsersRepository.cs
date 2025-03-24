using ChatsWebApi.Components.Types.Database;
using Microsoft.EntityFrameworkCore;

namespace ChatsWebApi.Components.Repositories.Users
{
    public class UsersRepository : IUsersRepository
    {
        private readonly AppDBContext _dbContext;

        public UsersRepository(AppDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> CreateAsync(User item)
        {
            User? userWithSameNickName = await _dbContext.Users.FirstOrDefaultAsync(u => u.NickName == item.NickName);
            if (userWithSameNickName != null)
                return null;

            await _dbContext.Users.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return false;
            
            user.NickName = null;
            user.Password = null;
            user.RefreshToken = null;
            user.IsDeleted = true;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
        }

        public async Task<User?> IsExistAsync(string NickName, string Password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.NickName == NickName && u.Password == Password);
        }

        public async Task<bool> SetRefreshTokenByIdAsync(string refreshToken, int id)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) 
                return false;

            user.RefreshToken = refreshToken;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
