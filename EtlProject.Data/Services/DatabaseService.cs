using EtlProject.Data.Models;
using Microsoft.Data.SqlClient;
using Dapper;

namespace EtlProject.Data.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task BulkInsertAsync(List<TripRecord> records)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            using var transaction = connection.BeginTransaction();

            try
            {
                const string sql = @"
                INSERT INTO Trips 
                (PickupDateTimeUtc, DropoffDateTimeUtc, 
                 PassengerCount, TripDistance, StoreAndFwdFlag, PULocationID, 
                 DOLocationID, FareAmount, TipAmount)
                VALUES 
                (@PickupDateTimeUtc, @DropoffDateTimeUtc,
                 @PassengerCount, @TripDistance, @StoreAndFwdFlag, @PULocationID,
                 @DOLocationID, @FareAmount, @TipAmount)";

                await connection.ExecuteAsync(sql, records, transaction);
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<int> GetRecordCountAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Trips");
        }

        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
