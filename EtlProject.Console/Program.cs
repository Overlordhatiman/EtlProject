using EtlProject.Console;
using EtlProject.Console.Orchestrator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = CreateHostBuilder(args).Build();

try
{
    IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    var csvFilePath = configuration["CsvFilePath"];

    if (string.IsNullOrEmpty(csvFilePath))
    {
        Console.WriteLine("CsvFilePath is not configured in appsettings.json");
        return;
    }

    if (!File.Exists(csvFilePath))
    {
        Console.WriteLine($"CSV file not found: {csvFilePath}");
        return;
    }

    Console.WriteLine($"Starting ETL process for: {csvFilePath}");

    await host.Services.GetRequiredService<IEtlOrchestrator>().ExecuteAsync(csvFilePath);

    Console.WriteLine("ETL process completed successfully!");
    Console.ReadKey();
}
catch (Exception ex)
{
    Console.WriteLine($"ETL process failed: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

static IHostBuilder CreateHostBuilder(string[] args) =>
Host.CreateDefaultBuilder(args)
.ConfigureServices((context, services) =>
{
    services.AddEtlServices(context.Configuration);
});