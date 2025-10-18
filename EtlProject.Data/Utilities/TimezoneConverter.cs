using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Utilities
{
    public class TimezoneConverter : ITimezoneConverter
    {
        private readonly TimeZoneInfo _estTimeZone;

        public TimezoneConverter()
        {
            try
            {
                _estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    _estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
                }
                catch (TimeZoneNotFoundException)
                {
                    _estTimeZone = TimeZoneInfo.CreateCustomTimeZone(
                        "EST",
                        TimeSpan.FromHours(-5),
                        "Eastern Standard Time",
                        "EST");
                }
            }
        }

        public DateTime ConvertToUtc(DateTime estDateTime)
        {
            if (estDateTime.Kind == DateTimeKind.Unspecified)
            {
                return TimeZoneInfo.ConvertTimeToUtc(estDateTime, _estTimeZone);
            }

            if (estDateTime.Kind == DateTimeKind.Utc)
            {
                return estDateTime;
            }

            return TimeZoneInfo.ConvertTimeToUtc(estDateTime, _estTimeZone);
        }
    }
}
