using CsvHelper;
using EtlProject.Data.Models;
using EtlProject.Data.Utilities;
using System.Globalization;

namespace EtlProject.Data.Services
{
    public class CsvProcessor : ICsvProcessor
    {
        private readonly ITimezoneConverter _timezoneConverter;

        public CsvProcessor(ITimezoneConverter timezoneConverter)
        {
            _timezoneConverter = timezoneConverter;
        }

        public async IAsyncEnumerable<TripRecord> ProcessCsvFileAsync(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            csv.Context.RegisterClassMap<CsvRecordMap>();

            await foreach (var record in csv.GetRecordsAsync<CsvRecord>())
            {
                yield return MapToTripRecord(record);
            }
        }

        private TripRecord MapToTripRecord(CsvRecord csvRecord)
        {
            var tripRecord = new TripRecord
            {
                PickupDateTimeUtc = _timezoneConverter.ConvertToUtc(csvRecord.tpep_pickup_datetime),
                DropoffDateTimeUtc = _timezoneConverter.ConvertToUtc(csvRecord.tpep_dropoff_datetime),
                PassengerCount = csvRecord.passenger_count,
                TripDistance = csvRecord.trip_distance,
                StoreAndFwdFlag = ConvertStoreAndFwdFlag(csvRecord.store_and_fwd_flag?.Trim()),
                PULocationID = csvRecord.PULocationID,
                DOLocationID = csvRecord.DOLocationID,
                FareAmount = csvRecord.fare_amount,
                TipAmount = csvRecord.tip_amount,
                DuplicateHash = GenerateDuplicateHash(csvRecord)
            };

            return tripRecord;
        }

        private static string ConvertStoreAndFwdFlag(string flag)
        {
            return flag?.ToUpper() switch
            {
                "Y" => "Yes",
                "N" => "No",
                _ => flag ?? "No"
            };
        }

        private static string GenerateDuplicateHash(CsvRecord record)
        {
            return $"{record.tpep_pickup_datetime:yyyyMMddHHmmss}_{record.tpep_dropoff_datetime:yyyyMMddHHmmss}_{record.passenger_count}";
        }

        public async Task<(List<TripRecord> unique, List<TripRecord> duplicates)> RemoveDuplicatesAsync(IEnumerable<TripRecord> records)
        {
            var seen = new HashSet<string>();
            var unique = new List<TripRecord>();
            var duplicates = new List<TripRecord>();

            foreach (var record in records)
            {
                if (seen.Add(record.DuplicateHash))
                {
                    unique.Add(record);
                }
                else
                {
                    duplicates.Add(record);
                }
            }

            return (unique, duplicates);
        }

        public async Task WriteDuplicatesToCsvAsync(List<TripRecord> duplicates, string outputPath)
        {
            using var writer = new StreamWriter(outputPath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            await csv.WriteRecordsAsync(duplicates);
        }
    }
}
