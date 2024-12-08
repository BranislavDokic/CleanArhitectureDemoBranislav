using Application.Interfaces.Repositoryinterfaces;
using Infrastructure.Database;
using Infrastructure.Repositories;
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
            return services;
        }
    }
}
