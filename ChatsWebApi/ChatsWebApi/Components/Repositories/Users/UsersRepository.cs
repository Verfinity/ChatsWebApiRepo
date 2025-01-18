﻿using ChatsWebApi.Components.Types.Database;
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

        public async Task<bool> DeleteAsync(User user)
        {
            if (user == null)
                return false;

            user.FirstName = null;
            user.LastName = null;
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

        public async Task<User?> IsUserExistAsync(string NickName, string Password)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.NickName == NickName && u.Password == Password);
        }

        public async Task<bool> SetRefreshTokenByNickNameAsync(string refreshToken, string nickName)
        {
            User? user = await _dbContext.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
            if (user == null) 
                return false;

            user.RefreshToken = refreshToken;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
