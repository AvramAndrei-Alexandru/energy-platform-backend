using EnergyDataPlatform.src.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Data.Repositories.Interfaces
{
    public interface ISensorMeasurementRepository
    {
        List<SensorMeasurement> GetAllSensorMeasurementsForDevice(Guid deviceId);
        SensorMeasurement AddSensorMeasurement(SensorMeasurement sensorMeasurement);
        int GetMeasurementCount(Guid deviceId);
        List<SensorMeasurement> GetMeasurementsForDeviceAndDate(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate);
        SensorMeasurement GetLastMeasurementValue(Guid deviceId);
        SensorMeasurement GetMaximumSensorMeasurementForDevice(Guid deviceId);
    }
}
