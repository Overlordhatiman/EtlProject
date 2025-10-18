using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Models
{
    public class TripRecord
    {
        public int Id { get; set; }
        public DateTime PickupDateTimeUtc { get; set; }
        public DateTime DropoffDateTimeUtc { get; set; }
        public int PassengerCount { get; set; }
        public decimal TripDistance { get; set; }
        public string? StoreAndFwdFlag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal FareAmount { get; set; }
        public decimal TipAmount { get; set; }
        public TimeSpan TravelTime { get; set; }

        // For duplicate detection
        public string? DuplicateHash { get; set; }
    }
}
