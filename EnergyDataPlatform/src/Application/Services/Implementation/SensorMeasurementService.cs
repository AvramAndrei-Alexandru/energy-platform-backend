using EnergyDataPlatform.src.Application.Mappers;
using EnergyDataPlatform.src.Application.Models;
using EnergyDataPlatform.src.Application.Services.Interfaces;
using EnergyDataPlatform.src.Data.Entities;
using EnergyDataPlatform.src.Data.Mesaging;
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
        private readonly IMessagingService _messagingService;

        public SensorMeasurementService(IMessagingService messagingService, ISensorMeasurementRepository sensorMeasurementRepository, ISmartDeviceRepository smartDeviceRepository)
        {
            _sensorMeasurementRepository = sensorMeasurementRepository;
            _smartDeviceRepository = smartDeviceRepository;
            _messagingService = messagingService;
            
        }

        public void AddSensorMeasurement(SensorMeasurementDashboardModel sensorMeasurement)
        {
           var measurement = _sensorMeasurementRepository.AddSensorMeasurement(SensorMeasurementMapper.ToSensorMeasurementEntity(sensorMeasurement));
           UpdateAverageConsumption(measurement);
        }

        public void AddSensorMeasurementFromExternalQueue(Message message)
        {
            var newMeasurement = SensorMeasurementMapper.ToSensorMeasurement(message);
            var lastMeasurement = _sensorMeasurementRepository.GetLastMeasurementValue(message.DeviceId);
            var maximumValue = _smartDeviceRepository.GetDeviceById(message.DeviceId).MaximumEnergyConsumption;
            if (lastMeasurement != null)
            {
                var timeDiff = newMeasurement.Timestamp.Subtract(lastMeasurement.Timestamp);
                var powerPeak = (newMeasurement.Measurement - lastMeasurement.Measurement) / ((decimal)timeDiff.TotalHours);
                if (powerPeak > maximumValue)
                {
                    _messagingService.SendMessageToAllClients(message, powerPeak);
                }
            }
            Console.WriteLine("Adding measurement with value " + newMeasurement.Measurement);
            _sensorMeasurementRepository.AddSensorMeasurement(newMeasurement);
            UpdateAverageConsumption(newMeasurement);
            _messagingService.SendUpdateMessage();
        }

        public List<SensorMeasurementDashboardModel> GetAllMeasurementsForDevice(Guid deviceId)
        {
           return _sensorMeasurementRepository.GetAllSensorMeasurementsForDevice(deviceId).Select(s => SensorMeasurementMapper.ToSensorMeasurementDashboardModel(s)).ToList();
        }

        public List<SensorMeasurementDashboardModel> GetDeviceHourlyMeasurementsInGivenInterval(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return GetHourlyAverageOfSensorMeasurementsInTimeInterval(deviceId, startDate, endDate).Select(m => SensorMeasurementMapper.ToSensorMeasurementDashboardModel(m)).ToList();
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

        private List<SensorMeasurement> GetHourlyAverageOfSensorMeasurementsInTimeInterval(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var allMeasurements = _sensorMeasurementRepository.GetMeasurementsForDeviceAndDate(deviceId, startDate, endDate);

            var groupMeasurements = allMeasurements.GroupBy(x => new { x.Timestamp.Year, x.Timestamp.Month, x.Timestamp.Day, x.Timestamp.Hour }).ToList();

            var filteredMeasurements = new List<SensorMeasurement>();

            foreach (var group in groupMeasurements)
            {
                if (!group.Any())
                {
                    continue;
                }
                decimal hourlyValue = 0;
                var measurementToAdd = new SensorMeasurement();
                measurementToAdd.Id = group.First().Id;
                var timeStamp = group.First().Timestamp;
                timeStamp = timeStamp.AddMinutes(-timeStamp.Minute);
                measurementToAdd.Timestamp = timeStamp;
                measurementToAdd.SmartDeviceId = group.First().SmartDeviceId;

                foreach (var item in group)
                {
                    hourlyValue += item.Measurement;
                }

                measurementToAdd.Measurement = hourlyValue / group.Count();
                filteredMeasurements.Add(measurementToAdd);
            }
            return filteredMeasurements;
        }

        private List<SensorMeasurement> ComputeBaseline(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var weeklyMeasurements = GetHourlyAverageOfSensorMeasurementsInTimeInterval(deviceId, startDate, endDate);

            var groupMeasurements = weeklyMeasurements.GroupBy(x => new { x.Timestamp.Hour }).OrderBy(g => g.Key.Hour).ToList();

            var filteredList = new List<SensorMeasurement>();

            foreach (var group in groupMeasurements)
            {
                if (!group.Any())
                {
                    continue;
                }

                decimal hourlyValue = 0;
                var measurementToAdd = new SensorMeasurement();
                measurementToAdd.Id = group.First().Id;
                var timeStamp = group.First().Timestamp;
                var processedTimeStamp = new DateTimeOffset(timeStamp.Year, timeStamp.Month, timeStamp.Day, group.First().Timestamp.Hour, 0, 0, endDate.Offset);
                measurementToAdd.Timestamp = processedTimeStamp;
                measurementToAdd.SmartDeviceId = group.First().SmartDeviceId;

                foreach (var item in group)
                {
                    hourlyValue += item.Measurement;
                }

                measurementToAdd.Measurement = hourlyValue / group.Count();
                filteredList.Add(measurementToAdd);
            }
            
            return filteredList.OrderBy(x => x.Timestamp).ToList();
        }

        public List<SensorMeasurementDashboardModel> GetWeekBaseline(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return ComputeBaseline(deviceId, startDate, endDate).Select(m => SensorMeasurementMapper.ToSensorMeasurementDashboardModel(m)).ToList();
        }

        public TimeToStartMeasurementModel GetBestTimeToStartAndMeasurements(int duration, Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var userBaseline = ComputeBaseline(deviceId, startDate, endDate);
            var maximumEnergyConsumption = _sensorMeasurementRepository.GetMaximumSensorMeasurementForDevice(deviceId).Measurement;
            var index = 0;
            var foundIndex = 0;

            foreach (var measurement in userBaseline)
            {
                measurement.Timestamp = measurement.Timestamp.AddDays(9);
            }

            var startTime = userBaseline.FirstOrDefault().Timestamp;
            var minimumPeak = userBaseline[0].Measurement + maximumEnergyConsumption;
            while (true)
            {
                if(index + duration >= 24)
                {
                    break;
                }
                var maximumPeakInInterval = userBaseline[index].Measurement + maximumEnergyConsumption;

                for (int i = index; i < index + duration; i++)
                {
                    if(userBaseline[i].Measurement + maximumEnergyConsumption > maximumPeakInInterval)
                    {
                        maximumPeakInInterval = userBaseline[i].Measurement + maximumEnergyConsumption;
                        
                    }
                }

                if(maximumPeakInInterval < minimumPeak)
                {
                    minimumPeak = maximumPeakInInterval;
                    startTime = userBaseline[index].Timestamp;
                    foundIndex = index;
                }
                index++;
            }

            var listToReturn = new List<SensorMeasurement>();

            for(int i = foundIndex; i <= foundIndex + duration; i++)
            {
                userBaseline[i].Measurement += maximumEnergyConsumption;
                listToReturn.Add(userBaseline[i]);
            }
            var returnedValue = new TimeToStartMeasurementModel
            {
                StartTime = startTime,
                EndTime = startTime.AddHours(duration),
                SensorMeasurements = listToReturn.Select(s => SensorMeasurementMapper.ToSensorMeasurementDashboardModel(s)).ToList()
            };
            return returnedValue;
        }
    }
}
