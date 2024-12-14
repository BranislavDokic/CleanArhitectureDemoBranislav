using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository :  IUserRepositoryInterface
    {
        private readonly RealDatabase _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRepository(RealDatabase dbContext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddAsync(User userToRegister)
        {
            if (!await _roleManager.RoleExistsAsync(userToRegister.Role))
            {
                await _roleManager.CreateAsync(new IdentityRole(userToRegister.Role));
            }
            try
            {
                var userCreated = await _userManager.CreateAsync(userToRegister);
                await _userManager.AddToRoleAsync(userToRegister, userToRegister.Role);
                return await Task.FromResult(userCreated);
            }
            catch (ArgumentException e)
            {
                throw new ArgumentException($"Failed to add user: {e.Message}");
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id, Func<IQueryable<User>, IQueryable<User>> include = null)
        {
            IQueryable<User> query = _dbContext.Users;

            if (include != null)
            {
                query = include(query);
            }

            return await query.FirstOrDefaultAsync(u => u.Id == id.ToString());
        }
        public async Task<string> DeleteAsync(Guid id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return "User not found.";
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return "User deleted successfully.";
        }

        public async Task<User> UpdateAsync(Guid id, User entity)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            _dbContext.Entry(user).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(string role)
        {
            return await _dbContext.Users.Where(u => u.Role == role).ToListAsync();
        }
    }
}
