﻿using ChatsWebApi.Components.Types.Database;
using ClassLibrary;
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

            _dbContext.Users.Remove(user);
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

        public async Task<bool> AddUserToChatAsync(int chatId, int userId)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (chat == null || user == null)
                return false;

            user.Chats.Add(chat);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserFromChatAsync(int chatId, int userId)
        {
            var chat = await _dbContext.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (chat == null || user == null || !user.Chats.Contains(chat))
                return false;

            user.Chats.Remove(chat);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
