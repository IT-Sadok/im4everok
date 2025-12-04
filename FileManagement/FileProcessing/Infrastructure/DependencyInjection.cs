using Application.Interfaces;
using Application.Interfaces.Repositories;

using Infrastructure.Database;
using Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetSection("DbConnection").Value,
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            return services;
        }
    }
}
