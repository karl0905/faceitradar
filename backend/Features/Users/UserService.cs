using FaceItRadar.Data;
using Microsoft.EntityFrameworkCore;

namespace FaceItRadar.Features.Users
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(string email, string password);
        Task<User?> GetUserByIdAsync(string userId);
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;

        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateUserAsync(string email, string password)
        {
            // Check if user already exists
            if (await _dbContext.Users.AnyAsync(u => u.email == email))
            {
                throw new InvalidOperationException("User with this email already exists");
            }

            // Create new user
            var user = new User
            {
                email = email,
                password_hash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            // Add to database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _dbContext.Users.FindAsync(userId);
        }
    }
}
