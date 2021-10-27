using EnergyDataPlatform.src.Application.Mappers;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Entities;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnergyDataPlatform.src.Application.Services.Implementation
{
    public class SensorMeasurementService : ISensorMeasurementService
    {
        private readonly ISensorMeasurementRepository _sensorMeasurementRepository;
        private readonly ISmartDeviceRepository _smartDeviceRepository;

        public SensorMeasurementService(ISensorMeasurementRepository sensorMeasurementRepository, ISmartDeviceRepository smartDeviceRepository)
        {
            _sensorMeasurementRepository = sensorMeasurementRepository;
            _smartDeviceRepository = smartDeviceRepository;
            
        }

        public void AddSensorMeasurement(SensorMeasurementDashboardModel sensorMeasurement)
        {
           var measurement = _sensorMeasurementRepository.AddSensorMeasurement(SensorMeasurementMapper.ToSensorMeasurementEntity(sensorMeasurement));
           UpdateAverageConsumption(measurement);
        }

        public List<SensorMeasurementDashboardModel> GetAllMeasurementsForDevice(Guid deviceId)
        {
           return _sensorMeasurementRepository.GetAllSensorMeasurementsForDevice(deviceId).Select(s => SensorMeasurementMapper.ToSensorMeasurementDashboardModel(s)).ToList();
        }

        public List<SensorMeasurementDashboardModel> GetDeviceMeasurementsForGivenDay(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return _sensorMeasurementRepository.GetMeasurementsForDeviceAndDate(deviceId, startDate, endDate).Select(s => SensorMeasurementMapper.ToSensorMeasurementDashboardModel(s)).ToList();
        }

        private void UpdateAverageConsumption(SensorMeasurement sensorMeasurement)
        {
            var count = _sensorMeasurementRepository.GetMeasurementCount(sensorMeasurement.SmartDeviceId);
            var device = _smartDeviceRepository.GetDeviceById(sensorMeasurement.SmartDeviceId);
            device.AverageEnergyConsumption = (device.AverageEnergyConsumption * (count - 1) + sensorMeasurement.Measurement) / count;
            _smartDeviceRepository.UpdateDevice(device);
        }
    }
}
