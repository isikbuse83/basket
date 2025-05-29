using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Data;
using ConsoleApp1.Domain;
using ConsoleApp1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DbContext = ConsoleApp1.Data.DbContext;

namespace ConsoleApp1.Services
{
    public class UserService
    {
        private readonly DbContext _dbContext;

        public UserService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, User updated)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return false;

            user.UserName = updated.UserName;
            user.Password = updated.Password;
            user.Email = updated.Email;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return false;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}