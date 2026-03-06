using Application.Features.Files.Commands.Delete;
using Application.Features.Files.Commands.Upload;
using Application.Features.Files.Queries.GetAll;
using Application.Interfaces;
using Application.Interfaces.External;
using Application.Interfaces.FileStorage;
using Application.Interfaces.MessageBrokers;
using Application.Interfaces.Repositories;

using Infrastructure.Database;
using Infrastructure.External;
using Infrastructure.FileStorage;
using Infrastructure.MessageBrokers;
using Infrastructure.Options;
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
            services.Configure<FileTextExtractionOptions>(configuration.GetSection(FileTextExtractionOptions.SectionName));
            services.Configure<AzureServiceBusOptions>(configuration.GetSection(AzureServiceBusOptions.SectionName));

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetSection("DbConnection").Value,
                sql => sql.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            ));

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IOutboxEventRepository, OutboxEventRepository>();
            services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            services.AddScoped<IFileStorage, AzureFileStorage>();
            services.AddScoped<IFileTextExtractionService, FileTextExtractionService>();

            services.AddScoped<UploadFileCommand>();
            services.AddScoped<DeleteFileCommand>();
            services.AddScoped<GetAllFilesQuery>();

            services.AddScoped<IPublisher, AzureServiceBusPublisher>();

            return services;
        }
    }
}
