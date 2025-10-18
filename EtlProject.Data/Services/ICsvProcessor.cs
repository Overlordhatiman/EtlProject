using EtlProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Services
{
    public interface ICsvProcessor
    {
        IAsyncEnumerable<TripRecord> ProcessCsvFileAsync(string filePath);
        Task<(List<TripRecord> unique, List<TripRecord> duplicates)> RemoveDuplicatesAsync(IEnumerable<TripRecord> records);
        Task WriteDuplicatesToCsvAsync(List<TripRecord> duplicates, string outputPath);
    }
}
