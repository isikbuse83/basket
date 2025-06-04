using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Domain.Entities;
using ConsoleApp1.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Application.Services
{
    public class UserService
    {
        private readonly BasketDbContext _basketDbContext;

        public UserService(BasketDbContext basketDbContext)
        {
            _basketDbContext = basketDbContext;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _basketDbContext.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _basketDbContext.Users.FindAsync(id);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _basketDbContext.Users.Add(user);
            await _basketDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, User updated)
        {
            var user = await _basketDbContext.Users.FindAsync(id);
            if (user == null) return false;

            user.Password = updated.Password;
            user.Email = updated.Email;
                
            await _basketDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _basketDbContext.Users.FindAsync(id);
            if (user == null) return false;

            _basketDbContext.Users.Remove(user);
            await _basketDbContext.SaveChangesAsync();
            return true;
        }
    }
}