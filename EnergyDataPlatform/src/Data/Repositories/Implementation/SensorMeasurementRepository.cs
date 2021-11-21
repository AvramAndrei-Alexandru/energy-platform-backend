using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnergyDataPlatform.src.Data.Entities;
using EnergyDataPlatform.src.Data.Persistance;
using EnergyDataPlatform.src.Data.Repositories.Interfaces;

namespace EnergyDataPlatform.src.Data.Repositories.Implementation
{
    public class SensorMeasurementRepository : ISensorMeasurementRepository
    {
        private readonly DatabaseContext _context;

        public SensorMeasurementRepository(DatabaseContext context)
        {
            _context = context;
        }

        public SensorMeasurement AddSensorMeasurement(SensorMeasurement sensorMeasurement)
        {
            sensorMeasurement.Id = Guid.NewGuid();
            _context.SensorMeasurements.Add(sensorMeasurement);
            _context.SaveChanges();
            return sensorMeasurement;
        }

        public List<SensorMeasurement> GetAllSensorMeasurementsForDevice(Guid deviceId)
        {
            return _context.SensorMeasurements.OrderBy(s => s.Timestamp).Where(s => s.SmartDeviceId == deviceId).ToList();
        }

        public SensorMeasurement GetLastMeasurementValue(Guid deviceId)
        {
            return _context.SensorMeasurements.OrderByDescending(s => s.Timestamp).FirstOrDefault(s => s.SmartDeviceId == deviceId);
        }

        public int GetMeasurementCount(Guid deviceId)
        {
            return _context.SensorMeasurements.Where(s => s.SmartDeviceId == deviceId).Count();
        }

        public List<SensorMeasurement> GetMeasurementsForDeviceAndDate(Guid deviceId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            return _context.SensorMeasurements.Where(s => s.SmartDeviceId == deviceId && (s.Timestamp >= startDate && s.Timestamp <= endDate)).OrderBy(s => s.Timestamp).ToList();
        }
    }
}
