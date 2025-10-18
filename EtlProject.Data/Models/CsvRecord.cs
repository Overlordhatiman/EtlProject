using CsvHelper.Configuration;
using EtlProject.Data.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Models
{
    public class CsvRecord
    {
        public DateTime tpep_pickup_datetime { get; set; }
        public DateTime tpep_dropoff_datetime { get; set; }
        public int passenger_count { get; set; }
        public decimal trip_distance { get; set; }
        public string? store_and_fwd_flag { get; set; }
        public int PULocationID { get; set; }
        public int DOLocationID { get; set; }
        public decimal fare_amount { get; set; }
        public decimal tip_amount { get; set; }
    }

    public sealed class CsvRecordMap : ClassMap<CsvRecord>
    {
        public CsvRecordMap()
        {
            Map(m => m.tpep_pickup_datetime)
            .Name("tpep_pickup_datetime")
            .TypeConverter<NullableDateTimeConverter>();

            Map(m => m.tpep_dropoff_datetime)
                .Name("tpep_dropoff_datetime")
                .TypeConverter<NullableDateTimeConverter>();

            Map(m => m.passenger_count)
                .Name("passenger_count")
                .TypeConverter<NullableInt32Converter>()
                .Default(0);

            Map(m => m.trip_distance)
                .Name("trip_distance")
                .TypeConverter<NullableDecimalConverter>()
                .Default(0m);

            Map(m => m.store_and_fwd_flag)
                .Name("store_and_fwd_flag")
                .TypeConverter<StringConverter>()
                .Default("N");

            Map(m => m.PULocationID)
                .Name("PULocationID")
                .TypeConverter<NullableInt32Converter>()
                .Default(0);

            Map(m => m.DOLocationID)
                .Name("DOLocationID")
                .TypeConverter<NullableInt32Converter>()
                .Default(0);

            Map(m => m.fare_amount)
                .Name("fare_amount")
                .TypeConverter<NullableDecimalConverter>()
                .Default(0m);

            Map(m => m.tip_amount)
                .Name("tip_amount")
                .TypeConverter<NullableDecimalConverter>()
                .Default(0m);
        }
    }
}
