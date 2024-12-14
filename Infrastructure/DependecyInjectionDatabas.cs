using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependecyInjectionDatabas
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionStrig)
        {
            
            services.AddDbContext<RealDatabase>(options =>
            {
                options.UseSqlServer(connectionStrig);
            });

            services.AddScoped(typeof(IGenericRepositoryInterface<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepositoryInterface, UserRepository>();
            services.AddIdentityCore<User>().AddRoles<IdentityRole>().AddEntityFrameworkStores<RealDatabase>();

            return services;
        }
    }
}
