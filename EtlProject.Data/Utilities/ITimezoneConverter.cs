using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EtlProject.Data.Utilities
{
    public interface ITimezoneConverter
    {
        DateTime ConvertToUtc(DateTime estDateTime);
    }
}
