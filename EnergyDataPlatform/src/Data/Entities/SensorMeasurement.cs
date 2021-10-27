using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Entities
{
    public class SensorMeasurement
    {
        public Guid Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public decimal Measurement { get; set; }
        public Guid SmartDeviceId { get; set; }
        public SmartDevice SmartDevice { get; set; }
    }
}
