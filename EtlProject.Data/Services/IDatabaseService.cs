using EtlProject.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Services
{
    public interface IDatabaseService
    {
        Task BulkInsertAsync(List<TripRecord> records);
        Task<int> GetRecordCountAsync();
    }
}
