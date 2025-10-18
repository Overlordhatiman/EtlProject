using EtlProject.Data.Models;
using EtlProject.Data.Services;
using Microsoft.Extensions.Logging;

namespace EtlProject.Console.Orchestrator
{
    public class EtlOrchestrator : IEtlOrchestrator
    {
        private readonly ICsvProcessor _csvProcessor;
        private readonly IDatabaseService _databaseService;
        private readonly ILogger<EtlOrchestrator> _logger;

        public EtlOrchestrator(ICsvProcessor csvProcessor, IDatabaseService databaseService, ILogger<EtlOrchestrator> logger)
        {
            _csvProcessor = csvProcessor;
            _databaseService = databaseService;
            _logger = logger;
        }

        public async Task ExecuteAsync(string csvFilePath)
        {
            _logger.LogInformation("Starting ETL process for {FilePath}", csvFilePath);

            var records = new List<TripRecord>();
            var batchSize = 10000;
            var batchNumber = 0;

            await foreach (var record in _csvProcessor.ProcessCsvFileAsync(csvFilePath))
            {
                records.Add(record);

                if (records.Count >= batchSize)
                {
                    await ProcessBatchAsync(records, batchNumber);
                    records.Clear();
                    batchNumber++;
                }
            }

            if (records.Any())
            {
                await ProcessBatchAsync(records, batchNumber);
            }

            _logger.LogInformation("ETL process completed successfully");
        }

        private async Task ProcessBatchAsync(List<TripRecord> records, int batchNumber)
        {
            _logger.LogInformation("Processing batch {BatchNumber} with {RecordCount} records", batchNumber, records.Count);

            var (unique, duplicates) = await _csvProcessor.RemoveDuplicatesAsync(records);

            if (duplicates.Any())
            {
                var duplicatesPath = $"duplicates_batch_{batchNumber}.csv";
                await _csvProcessor.WriteDuplicatesToCsvAsync(duplicates, duplicatesPath);
                _logger.LogInformation("Written {DuplicateCount} duplicates to {FilePath}", duplicates.Count, duplicatesPath);
            }

            await _databaseService.BulkInsertAsync(unique);
            _logger.LogInformation("Inserted {UniqueCount} records from batch {BatchNumber}", unique.Count, batchNumber);
        }
    }
}
