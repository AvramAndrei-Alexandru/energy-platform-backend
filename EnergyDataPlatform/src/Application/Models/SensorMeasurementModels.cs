using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Models
{
    public class SensorMeasurementDashboardModel
    {
        public Guid? Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public decimal Measurement { get; set; }
        public Guid SmartDeviceId { get; set; }
    }

    public class GetMeasurementModel
    {
        public Guid DeviceId { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }
}
