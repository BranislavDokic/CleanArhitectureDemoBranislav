using Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositoryinterfaces
{
    public interface IUserRepositoryInterface
    {
        Task<IdentityResult> AddAsync(User userToRegister, string password);
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(Guid id, Func<IQueryable<User>, IQueryable<User>> include = null);
        Task<string> DeleteAsync(Guid id); 
        Task<User> UpdateAsync(Guid id, User entity); 
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string role);
    }
}
