using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Utilities
{
    public class NullableInt32Converter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            if (int.TryParse(text, out int result))
                return result;

            return 0;
        }
    }

    public class NullableDecimalConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0m;

            if (decimal.TryParse(text, out decimal result))
                return result;

            return 0m;
        }
    }

    public class NullableDateTimeConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            if (string.IsNullOrWhiteSpace(text))
                return DateTime.MinValue;

            if (DateTime.TryParse(text, out DateTime result))
                return result;

            if (DateTime.TryParseExact(text, "MM/dd/yyyy hh:mm:ss tt",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                return result;

            return DateTime.MinValue;
        }
    }

    public class StringConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            return text?.Trim() ?? string.Empty;
        }
    }
}
