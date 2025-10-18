using EtlProject.Console.Orchestrator;
using EtlProject.Data.Services;
using EtlProject.Data.Utilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EtlProject.Console
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEtlServices(this IServiceCollection services, IConfiguration configuration)
        {
            var sqlConnectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddScoped<ICsvProcessor, CsvProcessor>();
            services.AddScoped<IDatabaseService, DatabaseService>(sp => new DatabaseService(sqlConnectionString));
            services.AddScoped<ITimezoneConverter, TimezoneConverter>();
            services.AddScoped<IEtlOrchestrator, EtlOrchestrator>();

            services.AddScoped(_ => new SqlConnection(sqlConnectionString));

            return services;
        }
    }
}
