using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependecyInjectionDatabas
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionStrig)
        {
            services.AddSingleton<FakeDatabas>();
            services.AddDbContext<RealDatabase>(options =>
            {
                options.UseSqlServer(connectionStrig);
            });
            return services;
        }
    }
}
