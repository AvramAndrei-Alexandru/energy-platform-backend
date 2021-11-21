using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Mesaging
{
    public class Message
    {
        public DateTimeOffset TimeStamp { get; set; }
        public Guid DeviceId { get; set; }
        public decimal MeasurementValue { get; set; }

        public override string ToString()
        {
            return "\nTimestamp = " + TimeStamp + ". Device ID = " + DeviceId + ". Measurement = " + MeasurementValue;  
        }
    }
}
