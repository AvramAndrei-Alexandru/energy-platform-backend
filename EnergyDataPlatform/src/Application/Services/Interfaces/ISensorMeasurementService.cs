using EnergyDataPlatform.src.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Interfaces
{
    public interface ISensorMeasurementService
    {
        List<SensorMeasurementDashboardModel> GetAllMeasurementsForDevice(Guid deviceId);
        void AddSensorMeasurement(SensorMeasurementDashboardModel sensorMeasurement);
        List<SensorMeasurementDashboardModel> GetDeviceMeasurementsForGivenDay(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}