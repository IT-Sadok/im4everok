using Application.Features.Files.Commands.Delete;
using Application.Features.Files.Commands.Upload;
using Application.Features.Files.Queries.GetAll;
using Application.Interfaces;
using Application.Interfaces.FileStorage;
using Application.Interfaces.Repositories;

using Infrastructure.Database;
using Infrastructure.FileStorage;
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
            services.Configure<FileStorageConfiguration>(configuration.GetSection(FileStorageConfiguration.SectionName));

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetSection("DbConnection").Value,
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IOutboxEventRepository, OutboxEventRepository>();
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddScoped<IFileStorage, AzureFileStorage>();

            services.AddScoped<UploadFileService>();
            services.AddScoped<DeleteFileService>();
            services.AddScoped<GetAllFilesService>();

            return services;
        }
    }
}
