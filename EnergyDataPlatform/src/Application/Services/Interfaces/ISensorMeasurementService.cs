using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Data.Mesaging;
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
        void AddSensorMeasurementFromExternalQueue(Message message);
        List<SensorMeasurementDashboardModel> GetDeviceHourlyMeasurementsInGivenInterval(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate);
        List<SensorMeasurementDashboardModel> GetWeekBaseline(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate);
        TimeToStartMeasurementModel GetBestTimeToStartAndMeasurements(int duration, Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}