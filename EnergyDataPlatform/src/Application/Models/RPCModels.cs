using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Models
{
    public class GetMeasurementsForDeviceInIntervalModel
    {
        public Guid DeviceID { get; set; }
        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
    }

    public class TimeToStartMeasurementModel
    {
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }
        public List<SensorMeasurementDashboardModel> SensorMeasurements { get; set; }
    }
}
