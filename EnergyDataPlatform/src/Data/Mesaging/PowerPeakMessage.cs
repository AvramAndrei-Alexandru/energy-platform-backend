using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Mesaging
{
    public class PowerPeakMessage
    {
        public string UserName { get; set; }
        public string DeviceName { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public decimal PowerPeak { get; set; }
    }
}
